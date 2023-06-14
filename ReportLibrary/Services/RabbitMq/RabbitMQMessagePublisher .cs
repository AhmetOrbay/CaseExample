using RabbitMQ.Client;
using ReportLibrary.Interfaces.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Services.RabbitMq
{
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly IModel _channel;

        public RabbitMQMessagePublisher(IModel channel)
        {
            _channel = channel;
        }

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "Reports", routingKey: "Report", basicProperties: null, body: body);
            Console.WriteLine("Message published to RabbitMQ: {0}", message);
        }
    }
    
}
