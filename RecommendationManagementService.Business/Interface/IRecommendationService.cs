using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Business.Interface
{
    public interface IRecommendationService
    {
        Task<Post> GetDailyRecommendation();
    }
}
