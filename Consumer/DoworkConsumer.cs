using System;
using System.Threading.Tasks;
using MassTransit;
using RabbitMqMassTransitMetrobusHangfire.Command;

namespace RabbitMqMassTransitMetrobusHangfire.Consumer
{
    public class DoworkConsumer:IConsumer<DoworkCommand>
    {
        public Task Consume(ConsumeContext<DoworkCommand> context)
        {
            Console.WriteLine($"Working {context.Message.Value}  - At:{context.Message.CreateAt:dd/MM/yyyy HH:mm:ss}");
            
            return Task.CompletedTask;
        }
    }
}