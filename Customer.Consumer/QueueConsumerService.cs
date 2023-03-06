using System.Runtime.CompilerServices;
using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customer.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _settings;
    
    public QueueConsumerService(IAmazonSQS sqs, IOptions<QueueSettings> settings)
    {
        _sqs = sqs;
        _settings = settings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await _sqs.GetQueueUrlAsync("customers", stoppingToken);

        var receiveMessageRequest = new ReceiveMessageRequest()
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            AttributeNames = new List<string> {"All"},
            MessageAttributeNames = new List<string> {"All"},
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

            foreach (var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;

                switch (messageType)
                {
                    case nameof(CustomerCreated):
                        var created = JsonSerializer.Deserialize<CustomerCreated>(message.Body);
                        break;
                    case nameof(CustomerUpdated):
                        var updated = JsonSerializer.Deserialize<CustomerUpdated>(message.Body);
                        break;
                    case nameof(CustomerDeleted):
                        var deleted = JsonSerializer.Deserialize<CustomerDeleted>(message.Body);
                        break;

                }
                
                await _sqs.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
            
            await Task.Delay(1000, stoppingToken);
        }
    }
}