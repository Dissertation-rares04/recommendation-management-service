using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Business.Interface
{
    public interface IUserInteractionService
    {
        Task SaveUserInteraction(UserInteraction userInteraction);

        Task SaveUserInteractions(List<UserInteraction> userInteractions);
    }
}
