using Confluent.Kafka;

namespace RecommendationManagementService.Business.Interface
{
    public interface IMessageHandler
    {
        Task HandleMessage(Message<Ignore, string> message);
    }
}
