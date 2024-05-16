using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.Model;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Business.Implementation
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IUserInteractionDataAccess _userInteractionDataAccess;

        public UserInteractionService(IUserInteractionDataAccess userInteractionDataAccess)
        {
            _userInteractionDataAccess = userInteractionDataAccess;
        }

        public async Task SaveUserInteraction(UserInteraction userInteraction)
        {
            await _userInteractionDataAccess.SaveUserInteraction(userInteraction);
        }

        public async Task SaveUserInteractions(List<UserInteraction> userInteractions)
        {
            await _userInteractionDataAccess.SaveUserInteractions(userInteractions);
        }
    }
}
