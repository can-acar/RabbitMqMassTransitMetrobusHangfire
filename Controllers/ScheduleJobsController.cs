using System;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMqMassTransitMetrobusHangfire.Command;

namespace RabbitMqMassTransitMetrobusHangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleJobsController : ControllerBase
    {
        private readonly IBusControl BusControl;

        public ScheduleJobsController(IBusControl busControl)
        {
            BusControl = busControl;
        }

        [HttpGet("doit")]
        public ActionResult<string> DoIt()
        {
            BusControl.Publish(new ScheduleJobCommand
            {
                Value = "Test",
                CreateAt = DateTime.Now
            });

            return Ok();
        }
    }
}