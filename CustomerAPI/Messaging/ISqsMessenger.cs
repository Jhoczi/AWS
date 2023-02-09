using Amazon.SQS.Model;

namespace ConsumerAPI.Messaging;

public interface ISqsMessenger
{
    Task<SendMessageResponse> SendMessageAsync<T>(T message);
}