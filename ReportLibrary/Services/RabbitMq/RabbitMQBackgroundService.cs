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
using System.Xml.Serialization;

namespace ReportLibrary.Services.RabbitMq
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        private readonly string queueName = "ReportMq";
        private readonly ConnectionFactory _connectionFactory;
        private readonly ReportDbContext _reportContext;
        private bool _isRunning;

        public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger
                            , ConnectionFactory connectionFactory
                            , ReportDbContext reportContext)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _reportContext = reportContext;
            _isRunning = false;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _isRunning = true;

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
                    AddReportDetail(modelDetail);
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                while (_isRunning)
                {
                    if (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
            }

            _logger.LogInformation("RabbitMQ background service has stopped.");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _isRunning = false;
            return base.StopAsync(cancellationToken);
        }

        public void AddReportDetail(ReportDetail modelDetail)
        {
            try
            {
                var entity = _reportContext.ReportDetails.Add(modelDetail);
                var report = _reportContext.Reports
                                    .FirstOrDefault(x => x.Id == modelDetail.ReportId);
                if (report is not null)
                {
                    report.Status = ReportStatus.Completed;
                    _reportContext.Entry(report).Property(x => x.Status).IsModified = true;
                }
                else
                {

                    _logger.LogError($"not found ReportsId ");

                    _reportContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error => {ex.Message}");
            }
        }
    }
}
