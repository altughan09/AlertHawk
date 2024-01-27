﻿using AlertHawk.Monitoring.Domain.Interfaces.Repositories;
using AlertHawk.Monitoring.Infrastructure.MonitorManager;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

namespace AlertHawk.Monitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitorController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private IMonitorManager _monitorManager;

        public MonitorController(IPublishEndpoint publishEndpoint, IMonitorManager monitorManager)
        {
            _publishEndpoint = publishEndpoint;
            _monitorManager = monitorManager;
        }

        [HttpGet]
        public async Task StartMonitor()
        {
            await _monitorManager.Start();
        }

        [HttpGet("monitorStatus")]
        public IActionResult MonitorStatus()
        {
            return Ok($"Master Node: {GlobalVariables.MasterNode}");
        }

        [HttpPost]
        public async Task<IActionResult> ProduceNotification(int notificationId, string message, int messageQuantity)
        {
            if (messageQuantity > 50)
            {
                messageQuantity = 50;
            }

            for (int i = 0; i < messageQuantity; i++)
            {
                await _publishEndpoint.Publish<NotificationAlert>(new
                {
                    NotificationId = notificationId,
                    TimeStamp = DateTime.UtcNow,
                    Message = message + "_" + i
                });
            }

            return Ok($"{messageQuantity} Messages sent");
        }
    }
}