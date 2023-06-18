using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Services.RabbitMq
{
    public class PublishRabbitMQService
    {
        //private readonly string queueName = "ReportRequest";
        private readonly ConnectionFactory _connectionFactory;

        public PublishRabbitMQService(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool PublishMessage(string message,string queueName)
        {
            try
            {
                using (var connection = _connectionFactory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);

                    Console.WriteLine("Send Messages: {0}", message);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
            
        }
    }
}
