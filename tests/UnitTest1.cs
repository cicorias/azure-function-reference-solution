using Azure.Messaging;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Time.Testing;
using Xunit;
using Moq;

using AzFunctionsEgEhSbStg.Functions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace FunctionTests;

public class UnitTest1
{

    [Fact]
    public async Task Test1()
    {
        var when = new DateTimeOffset(2021, 01, 01, 0, 0, 0, TimeSpan.Zero);
        var clock = new FakeTimeProvider(when);

        var mockLogger = new Mock<ILogger<DefenderDoneHandler>>();
        var nullLogger = NullLogger<DefenderDoneHandler>.Instance;
        var nl = NullLoggerFactory.Instance.CreateLogger<DefenderDoneHandler>();

        var eventData = new { stuff = "foobar" };
        
        var cloudEvent = new CloudEvent(
            source: "foobar",
            
            type: "testEvent",
            data: new BinaryData(eventData),
            dataContentType: "application/json",
            dataFormat: CloudEventDataFormat.Json)
            {
                Subject = "foobar",
                Time = clock.GetUtcNow()
            };



        var DefenderDoneHandler = new DefenderDoneHandler(nl); //mockLogger.Object);

         // Act
        var foo = await DefenderDoneHandler.RunAsync(cloudEvent);
        var body = foo.EventBody;
        var props = foo.Properties;

        // Assert that props contains the header
        Assert.Equal("YourHeaderInformation", props["Header"]);

        // mockLogger.Verify(
        //     x => x.Log(
        //         LogLevel.Information,
        //         It.IsAny<EventId>(),
        //         It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"Event Grid event: {foo.EventBody}")),
        //         null,
        //         // (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
        //         It.IsAny<Func<It.IsAnyType, Exception, string>>()),
        //     Times.Once);

        // Assert.Equal("{\"Header\":\"YourHeaderInformation\",\"EventType\":\"testEvent\",\"EventSubject\":\"foobar\"}", foo);
        // Assert.Equal("foobar", );
        // Assert.Equal("your-source", foo.Source);

        //assert time is good
        
        Assert.Equal(when, cloudEvent.Time);


    }
}