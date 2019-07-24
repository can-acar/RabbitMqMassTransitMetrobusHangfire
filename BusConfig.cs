using System;
using MetroBus;
using MetroBus.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RabbitMqMassTransitMetrobusHangfire.Consumer;

namespace RabbitMqMassTransitMetrobusHangfire
{
    public static class BusConfig
    {
        public static void UseBusService(this IServiceCollection services, MqConstant mqConstant)
        {
            services.AddMetroBus(x => x.AddConsumer<ScheduleJobConsumer>());
            services.AddMetroBus(x => x.AddConsumer<DoworkConsumer>());


            services.AddSingleton(provider => MetroBusInitializer.Instance
                    .UseRabbitMq(mqConstant.RabbitMQUri, mqConstant.RabbitMQUserName, mqConstant.RabbitMQPassword)
                    .RegisterConsumer<ScheduleJobConsumer>("schedule.queue", provider)
                    .UseRetryPolicy()
                    .UseIncrementalRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10))
                    .Then()
                    .UseCircuitBreaker(10, 5, TimeSpan.FromSeconds(10))
                    .RegisterConsumer<DoworkConsumer>("dowork.queue", provider)
                    .UseRetryPolicy()
                    .UseIncrementalRetryPolicy(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10))
                    .Then()
                    .UseCircuitBreaker(10, 5, TimeSpan.FromSeconds(10))
                    .Build())
                .BuildServiceProvider();

            services.AddHostedService<BusService>();
        }
    }
}