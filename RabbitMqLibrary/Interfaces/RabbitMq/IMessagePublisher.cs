using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Interfaces.RabbitMq
{
    public interface IMessagePublisher
    {
        void Publish(string message);

    }
}
