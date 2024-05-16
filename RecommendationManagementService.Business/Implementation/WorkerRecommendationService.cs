using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.Model;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Business.Implementation
{
    public class WorkerRecommendationService: IWorkerRecommendationService
    {
        private readonly IRecommendationServiceDataAccess _recommendationServiceDataAccess;
        private readonly IUserInteractionDataAccess _userInteractionDataAccess;

        public WorkerRecommendationService(IRecommendationServiceDataAccess recommendationServiceDataAccess, IUserInteractionDataAccess userInteractionDataAccess)
        {
            _recommendationServiceDataAccess = recommendationServiceDataAccess;
            _userInteractionDataAccess = userInteractionDataAccess;
        }

        public async Task ComputeRecommendationsForUser(string userId)
        {
            var mostPopularCategoryForUser = await _userInteractionDataAccess.GetMostPopularCategoryForUser(userId);

            var mostPopularPostsForCategory = await _userInteractionDataAccess.GetMostPopularPostsForCategory(mostPopularCategoryForUser);

            var postsUserHasInteractedWith = await _userInteractionDataAccess.GetPostsForCategoryUserHasInteractedWith(mostPopularCategoryForUser, userId);

            var userRecommendations = mostPopularPostsForCategory
                .Except(postsUserHasInteractedWith)
                .Select(postId => new UserRecommendation { UserId = userId, PostId = postId })
                .ToList();

            // If user has already interacted with all popular posts from the category, suggest a random post from that category
            if (userRecommendations.Count < 10)
            {
                var latestPostsByCategory = await _recommendationServiceDataAccess.GetLatestPostsByCategory(mostPopularCategoryForUser);

                var newRecommendations = latestPostsByCategory
                    .Select(x => x.Id)
                    .Except(postsUserHasInteractedWith)
                    .Select(postId => new UserRecommendation { UserId = userId, PostId = postId })
                    .ToList();

                userRecommendations.AddRange(newRecommendations);
            }

            // If user has already interacted with all posts from that category, suggest a random post from next most popular category of the user

            // Etc...

            if (userRecommendations.Any())
            {
                await _recommendationServiceDataAccess.SaveUserRecommendations(userRecommendations);
            }
        }
    }
}
