using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var customer = new CustomerCreated()
{
    Id = Guid.NewGuid(),
    Email = "hoczispoczi@gmail.com",
    FullName = "Jakub Hoczek",
    DateOfBirth = new DateTime(1998, 1, 6),
    GitHubUsername = "Jhoczi",
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("customers");

var sendMessageReq = new SendMessageRequest()
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(customer),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue()
            {
                DataType = "String",
                StringValue = nameof(CustomerCreated)
            }
        }
    }
};

var response = await sqsClient.SendMessageAsync(sendMessageReq);
Console.WriteLine(response);