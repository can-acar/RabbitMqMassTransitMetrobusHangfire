using System;

namespace RabbitMqMassTransitMetrobusHangfire.Command
{
    public class DoworkCommand
    {
        public string Value { get; set; }
        public DateTime CreateAt { get; set; }
    }
}