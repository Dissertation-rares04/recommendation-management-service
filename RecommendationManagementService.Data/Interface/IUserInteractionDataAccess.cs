using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Data.Interface
{
    public interface IUserInteractionDataAccess
    {
        Task<List<UserInteraction>> FindUserInteraction(string postId, string userId);

        Task<List<UserInteraction>> GetAllUserInteractions(string userId);

        Task<string> GetMostPopularCategoryForUser(string userId);

        Task<List<string>> GetMostPopularPostsForCategory(string category);

        Task<List<string>> GetPostsForCategoryUserHasInteractedWith(string category, string userId);

        Task SaveUserInteraction(UserInteraction userInteraction);

        Task SaveUserInteractions(List<UserInteraction> userInteractions);
    }
}
