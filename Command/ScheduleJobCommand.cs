using System;

namespace RabbitMqMassTransitMetrobusHangfire.Command
{
    public class ScheduleJobCommand
    {
        public string Value { get; set; }
        public DateTime CreateAt { get; set; }
    }
}