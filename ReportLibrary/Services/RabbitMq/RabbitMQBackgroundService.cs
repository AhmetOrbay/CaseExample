using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using ReportLibrary.Model;
using ReportLibrary.Repositories;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ReportLibrary.Interfaces;

namespace ReportLibrary.Services.RabbitMq
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        private readonly string queueName = "ReportMq";
        private readonly ConnectionFactory _connectionFactory;
        private readonly IReportService _reportService;

        public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger
                            , ConnectionFactory connectionFactory
                            , IReportService reportService)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _reportService = reportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            stoppingToken.Register(() =>
                _logger.LogInformation("RabbitMQ background service is stopping."));

            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var modelDetail = JsonSerializer.Deserialize<ReportDetail>(message);
                    _reportService.AddReportDetail(modelDetail);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }

            _logger.LogInformation("RabbitMQ background service has stopped.");
        }
    }
}
