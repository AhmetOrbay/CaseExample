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
using HotelLibrary.Repositories;
using HotelLibrary.Model;
using Microsoft.EntityFrameworkCore;
using HotelLibrary.Models;

namespace HotelLibrary.Services.RabbitMq
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly ILogger<RabbitMQBackgroundService> _logger;
        private readonly string queueName = "ReportRequest";
        private readonly string queuePublishName = "ReportMq";
        private readonly ConnectionFactory _connectionFactory;
        private readonly HotelDbContext _HotelDbContext;
        private readonly PublishRabbitMQService _publishRabbitMQService;

        public RabbitMQBackgroundService(ILogger<RabbitMQBackgroundService> logger, ConnectionFactory connectionFactory, HotelDbContext HotelDbContext,
          PublishRabbitMQService publishRabbitMQService)
        {
            _logger = logger;
            _connectionFactory = connectionFactory;
            _HotelDbContext = HotelDbContext;
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
                consumer.Received += (model, ea) => {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("incoming message: {0}", message);
                    var ConsumeData = JsonSerializer.Deserialize<ConsumeModel>(message);
                    var publish = GetReportDetails(ConsumeData);
                    _publishRabbitMQService.PublishMessage(JsonSerializer.Serialize(publish), queuePublishName);
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

        /// <summary>
        /// Get Report Detail Created
        /// </summary>
        /// <param name="Consume"></param>
        /// <returns></returns>
        private ReportDetail GetReportDetails(ConsumeModel Consume)
        {
            ReportDetail response = new();
            try
            {
                var hotel = GetHotel(Consume.HotelId);
                var dbData = GetHotelList(Consume, hotel.Address.District);
                response = new ReportDetail()
                {
                    City = Consume.City,
                    Country = Consume.Country,
                    District = Consume.District,
                    GoogleLocation = Consume.GoogleLocation,
                    LocationHotelCount = dbData.Count(),
                    LocationTelephoneCount = dbData.Sum(x => x.HotelContacts.Count()),
                    ReportId = Consume.ReportId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Report Detail Error => {ex.Message}");
            }
            return response;
        }

        /// <summary>
        /// Get Hotel By Id
        /// </summary>
        /// <param name="HotelId"></param>
        /// <returns></returns>
        private Hotel GetHotel(long HotelId)
        {
            var hotel = _HotelDbContext.Hotels
                    .Include(x => x.Address).ThenInclude(x => x.District)
                    .ThenInclude(x => x.City)
                    .ThenInclude(x => x.Country)
                    .FirstOrDefault(x => x.Id == HotelId);

            return hotel;
        }


        /// <summary>
        /// Get Hotel List
        /// </summary>
        /// <param name="Consume"></param>
        /// <param name="District"></param>
        /// <returns></returns>
        private List<Hotel> GetHotelList(ConsumeModel Consume,District District)
        {
            var query = _HotelDbContext.Hotels
                  .Include(x => x.HotelContacts)
                  .Include(x => x.Address)
                  .ThenInclude(x => x.District)
                  .ThenInclude(x => x.City)
                  .ThenInclude(x => x.Country)
                  .Include(x => x.HotelContacts)
                  .Where(x => x.Id == Consume.HotelId)
                  .AsQueryable();
            query = query.Where(x => x.Address.District.Name == District.Name).AsQueryable();
            query = query.Where(x => x.Address.District.City.Name == District.City.Name).AsQueryable();
            query = query.Where(x => x.Address.District.City.Country.Name == District.City.Country.Name).AsQueryable();

            var dbData = query.ToList();
            return dbData;
        }
    }
}