using Confluent.Kafka;

namespace RecommendationManagementService.Core.AppSettings
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }


        public string GroupId { get; set; }


        public SecurityProtocol SecurityProtocol { get; set; }


        public SaslMechanism SaslMechanism { get; set; }


        public string SaslUsername { get; set; }


        public string SaslPassword { get; set; }
    }
}
