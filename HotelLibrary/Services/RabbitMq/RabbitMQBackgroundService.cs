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
using HotelLibrary.Models.RabbitMq;
using HotelLibrary.Interfaces;

namespace HotelLibrary.Services.RabbitMq
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        private readonly string queueName = "ReportRequest";
        private readonly ConnectionFactory _connectionFactory;
        private readonly IHotelService _hotelService;
        private readonly PublishRabbitMQService _publishRabbitMQService;

        public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger
                            , ConnectionFactory connectionFactory
                            , IHotelService hotelService,
                              PublishRabbitMQService publishRabbitMQService)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _hotelService = hotelService;
            _publishRabbitMQService = publishRabbitMQService;
            
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
                    _logger.LogInformation("incoming message: {0}", message);
                    var ConsumeData = JsonSerializer.Deserialize<ConsumeModel>(message);
                    var publish = _hotelService.GetReportDetails(ConsumeData);
                    _publishRabbitMQService.PublishMessage(JsonSerializer.Serialize(publish));
                    _logger.LogInformation("Publish Data message: {0}", message);
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
