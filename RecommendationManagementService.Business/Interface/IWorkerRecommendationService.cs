namespace RecommendationManagementService.Business.Interface
{
    public interface IWorkerRecommendationService
    {
        Task ComputeRecommendationsForUser(string userId);
    }
}
