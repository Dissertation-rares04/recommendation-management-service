using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Data.Interface
{
    public interface IRecommendationServiceDataAccess
    {
        Task<List<Post>> GetLatestPostsByCategory(string category);

        Task SaveUserRecommendations(List<UserRecommendation> userRecommendations);
    }
}
