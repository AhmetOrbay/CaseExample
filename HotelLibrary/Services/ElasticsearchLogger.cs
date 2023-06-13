using Elasticsearch.Net;
using HotelLibrary.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HotelLibrary.Services
{
    public class ElasticsearchLogger : ILogger
    {

        private readonly ElasticLowLevelClient _elasticClient;
        private readonly string _defaultIndex;

        public ElasticsearchLogger(IOptions<ElasticSettings> elasticsearchSettings)
        {
            var settings = elasticsearchSettings.Value;
            _elasticClient = new ElasticLowLevelClient(new ConnectionConfiguration(new Uri(settings.Url)));
            _defaultIndex = settings.DefaultIndex;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            throw new NotImplementedException();
        }
    }
}
