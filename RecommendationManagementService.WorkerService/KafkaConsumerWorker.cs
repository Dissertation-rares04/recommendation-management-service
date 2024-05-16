using Confluent.Kafka;
using Microsoft.Extensions.Options;
using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.AppSettings;

namespace RecommendationManagementService.WorkerService
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<Ignore, string> _consumer;

        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly IMessageHandler _messageHandler;

        public KafkaConsumerWorker(IOptions<KafkaSettings> kafkaSettings, IMessageHandler messageHandler, ILogger<KafkaConsumerWorker> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "recommendation-management-service",
                SecurityProtocol = SecurityProtocol.Plaintext,
                //SecurityProtocol = kafkaSettings.Value.SecurityProtocol,
                //SaslMechanism = kafkaSettings.Value.SaslMechanism,
                //SaslUsername = kafkaSettings.Value.SaslUsername,
                //SaslPassword = kafkaSettings.Value.SaslPassword
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(_consumerConfig)
                .Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe("user_interactions");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Waiting to consume...");

                    var message = _consumer.Consume(stoppingToken);

                    await _messageHandler.HandleMessage(message.Message);
                }
                catch (OperationCanceledException)
                {
                    // Graceful shutdown
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred: {ex.Message}");
                }
            }

            _consumer.Close();
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _consumer.Close();
            await base.StopAsync(cancellationToken);
        }
    }
}
