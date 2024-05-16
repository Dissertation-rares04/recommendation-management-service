using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Data.Interface
{
    public interface IRecommendationServiceDataAccess
    {
        Task<List<Post>> GetLatestPostsByCategory(string category);

        Task<List<Post>> GetRecentPostsByUserId(string userId);

        Task<List<Post>> GetRecentPostsByTopic(string topic);

        Task SaveUserRecommendations(List<UserRecommendation> userRecommendations);
    }
}
