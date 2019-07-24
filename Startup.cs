using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RabbitMqMassTransitMetrobusHangfire
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        IHostingEnvironment HostingEnvironment { get; }

        public ILifetimeScope ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;

            HostingEnvironment = environment;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddHangfire(configuration =>
                configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    
                    .UseSqlServerStorage(Configuration.GetConnectionString("HangFireDbConnection"),
                        new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            UsePageLocksOnDequeue = true,
                            DisableGlobalLocks = true
                        }));
            
            services.AddHangfireServer();
            
            services.AddSingleton<IBackgroundJobClient, BackgroundJobClient>();

            services.Configure<IISOptions>(options => { options.ForwardClientCertificate = true; });

            services.UseBusService(new MqConstant(Configuration));


            var builder = new ContainerBuilder();

            builder.Populate(services);

            ApplicationContainer = ConfigureContainer(builder).Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private ContainerBuilder ConfigureContainer(ContainerBuilder builder)
        {
            return builder;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(serviceProvider);
            GlobalConfiguration.Configuration.UseAutofacActivator(ApplicationContainer, false);

            DashboardOptions opts = new DashboardOptions
            {
                Authorization = new[]
                {
                    new NoAuthorizationFilter(),
                },
                AppPath = "https://localhost:5001"
            };
            app.UseHangfireDashboard("/jobs", opts);

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
    
    public class NoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true; //!string.IsNullOrEmpty(context.Request.RemoteIpAddress) && (context.Request.RemoteIpAddress == "127.0.0.1" || context.Request.RemoteIpAddress == "::1" || context.Request.RemoteIpAddress == context.Request.LocalIpAddress);
        }
    }
}