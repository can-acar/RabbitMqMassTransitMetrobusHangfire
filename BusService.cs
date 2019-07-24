

using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace RabbitMqMassTransitMetrobusHangfire
{
    public class BusService : IHostedService
    {
        private readonly IBusControl Control;

        public BusService(IBusControl control)
        {
            Control = control;

        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {

            await Control.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Control.StopAsync(cancellationToken);
        }
    }
}