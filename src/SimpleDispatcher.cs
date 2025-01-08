using System;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace src
{
    public class SimpleDispatcher
    {
        private readonly ILogger<SimpleDispatcher> _logger;

        public SimpleDispatcher(ILogger<SimpleDispatcher> logger)
        {
            _logger = logger;
        }

        [Function(nameof(SimpleDispatcher))]
        public void Run([EventHubTrigger("samples-workitems", Connection = "")] EventData[] events)
        {
            foreach (EventData @event in events)
            {
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }
        }
    }
}
