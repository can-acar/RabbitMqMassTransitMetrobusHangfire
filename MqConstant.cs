using Microsoft.Extensions.Configuration;

namespace RabbitMqMassTransitMetrobusHangfire
{
    public class MqConstant
    {
        private readonly IConfiguration Configuration;

        public MqConstant(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string RabbitMQUri => Configuration["EventBusConnection"];
        public string RabbitMQUserName => Configuration["EventBusUserName"];
        public string RabbitMQPassword => Configuration["EventBusPassword"]; 
    }
}