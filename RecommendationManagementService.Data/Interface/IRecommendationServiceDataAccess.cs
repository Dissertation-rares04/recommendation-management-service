using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Data.Interface
{
    public interface IRecommendationServiceDataAccess
    {
        Task<List<Post>> GetRecentPostsByUserId(string userId);

        Task<List<Post>> GetRecentPostsByTopic(string topic);
    }
}
