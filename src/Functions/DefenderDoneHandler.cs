// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Reflection.Metadata;
using Azure.Messaging;
using Azure.Messaging.EventHubs;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Mime;

namespace AzFunctionsEgEhSbStg.Functions
{
    public class DefenderDoneHandler(ILogger<DefenderDoneHandler> logger)
    {
        static readonly string SOURCE = "defender-done-handler-function";
        private readonly ILogger<DefenderDoneHandler> _logger = logger;

        [Function(nameof(DefenderDoneHandler))]
        [EventHubOutput("your-eventhub-name", Connection = "EventHubConnectionAppSetting")]
        public async Task<EventData> RunAsync([EventGridTrigger] CloudEvent cloudEvent)
        {
            _logger.LogInformation("Event type: {type}, Event subject: {subject}", cloudEvent.Type, cloudEvent.Subject);

            // Create the return object with header information
            // use the CloudEvent(string source, string type, object? jsonSerializableData, Type? dataSerializationType = null) constructor
            var ce = new CloudEvent(SOURCE, "your-type", new { stuff = "foobar" }){
                Subject = cloudEvent.Subject,
                Time = DateTimeOffset.UtcNow
            };

            var eventData = new EventData(new BinaryData(ce)){
                Properties = {
                    { "Header", "YourHeaderInformation" }
                },
                ContentType = MediaTypeNames.Application.Json
            };

            await Task.CompletedTask; // Placeholder for async operations

            return eventData;
        }
    }
}
