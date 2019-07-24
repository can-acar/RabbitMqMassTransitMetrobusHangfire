using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMqMassTransitMetrobusHangfire.Command;


namespace RabbitMqMassTransitMetrobusHangfire.Jobs
{
    public class ScheduleJob
    {
        
        private readonly ILogger<ScheduleJob> Logger;
        private readonly IBusControl BusControl;

        public ScheduleJob(ILogger<ScheduleJob> logger,IBusControl busControl)
        {
            Logger = logger;
            BusControl = busControl;
        }
      

        public Task DoIt(string value,DateTime createAt)
        {
         return   BusControl.Publish(new DoworkCommand
            {
                Value = value,
                CreateAt = createAt
            });
            
         
        }
    }
}