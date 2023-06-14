using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportLibrary.Interfaces.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Services.RabbitMq
{
    public class RabbitMQMessageConsumer : IMessageConsumer
    {
        private readonly IModel _channel;

        public RabbitMQMessageConsumer(IModel channel)
        {
            _channel = channel;
        }       

        public void ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Message received from RabbitMQ: {0}", message);

                _channel.BasicAck(ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "Report", autoAck: false, consumer: consumer);
            Console.WriteLine("Listening for messages from RabbitMQ...");
        }
    }
}
