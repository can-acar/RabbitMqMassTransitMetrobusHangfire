using System;
using System.Threading.Tasks;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMqMassTransitMetrobusHangfire.Command;
using RabbitMqMassTransitMetrobusHangfire.Jobs;

namespace RabbitMqMassTransitMetrobusHangfire.Consumer
{
    public class ScheduleJobConsumer : IConsumer<ScheduleJobCommand>
    {
        private readonly ILogger<ScheduleJobConsumer> Logger;
        private readonly IBackgroundJobClient JobClient;

        public ScheduleJobConsumer(ILogger<ScheduleJobConsumer> logger, IBackgroundJobClient jobClient)
        {
            Logger = logger;
            JobClient = jobClient;
        }

        public async Task Consume(ConsumeContext<ScheduleJobCommand> context)
        {
            var Value = context.Message.Value;
            var CreateAt = context.Message.CreateAt;
            var nextSchedule = DateTime.Now.AddSeconds(10);
            
            JobClient.Schedule<ScheduleJob>(x => x.DoIt(Value, CreateAt),nextSchedule);
        }
    }
}