using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLibrary.Services.RabbitMq
{
    public class PublishRabbitMQService
    {
        private readonly string queueName = "ReportMq";
        private readonly ConnectionFactory _connectionFactory;

        public PublishRabbitMQService(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void PublishMessage(string message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                Console.WriteLine("Send Messages: {0}", message);
            }
        }
    }
}
