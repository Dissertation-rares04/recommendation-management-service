using Confluent.Kafka;
using RecommendationManagementService.Business.Interface;

namespace RecommendationManagementService.WorkerService
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly IConsumer<Ignore, string> _consumer;

        private readonly ILogger<KafkaConsumerWorker> _logger;
        private readonly IMessageHandler _messageHandler;

        public KafkaConsumerWorker(IMessageHandler messageHandler, ILogger<KafkaConsumerWorker> logger)
        {
            _messageHandler = messageHandler;
            _logger = logger;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "recommendation-management-service",
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
