using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Business.Implementation
{
    public class UserInteractionMessageHandler : IMessageHandler
    {
        private readonly ILogger<UserInteractionMessageHandler> _logger;

        private readonly IUserInteractionService _userInteractionService;
        private readonly IWorkerRecommendationService _workerRecommendationService;

        private Dictionary<string, List<UserInteraction>> _userInteractions;

        public UserInteractionMessageHandler(IUserInteractionService userInteractionService, IWorkerRecommendationService workerRecommendationService, ILogger<UserInteractionMessageHandler> logger)
        {
            _userInteractionService = userInteractionService;
            _workerRecommendationService = workerRecommendationService;
            _logger = logger;

            _userInteractions = new Dictionary<string, List<UserInteraction>>();
        }

        public async Task HandleMessage(Message<Ignore, string> message)
        {
            var userInteraction = JsonConvert.DeserializeObject<UserInteraction>(message.Value);

            // Check if the key exists
            var key = userInteraction.UserId;
            if (_userInteractions.ContainsKey(key))
            {
                // Key exists, retrieve the list and add the new interaction
                _userInteractions[key].Add(userInteraction);

                if (_userInteractions[key].Count >= 5)
                {
                    try
                    {
                        await _userInteractionService.SaveUserInteractions(_userInteractions[key]);
                        await _workerRecommendationService.ComputeRecommendationsForUser(key);
                    }
                    catch (Exception ex)
                    {

                        _logger.LogError(ex.Message);
                    }
                    finally
                    {
                        _userInteractions.Remove(key);
                    }
                }
            }
            else
            {
                // Key doesn't exist, create a new list, add the interaction, and add the key-value pair
                List<UserInteraction> newList = new()
                {
                    userInteraction
                };
                _userInteractions.Add(key, newList);
            }

            _logger.LogInformation(JsonConvert.SerializeObject(userInteraction));
        }
    }
}
