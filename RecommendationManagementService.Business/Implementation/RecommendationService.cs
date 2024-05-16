using RecommendationManagementService.Business.Interface;
using RecommendationManagementService.Core.Model;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Business.Implementation
{
    public class RecommendationService : BaseService, IRecommendationService
    {
        private readonly IRecommendationServiceDataAccess _recommendationServiceDataAccess;

        public RecommendationService(IRecommendationServiceDataAccess recommendationServiceDataAccess, IUserResolver userResolver) : base(userResolver)
        {
            _recommendationServiceDataAccess = recommendationServiceDataAccess;
        }

        public async Task<Post> GetDailyRecommendation()
        {
            // Get posts user has recently interracted with (liked, commented on, viewed)
            var userRecentPosts = await _recommendationServiceDataAccess.GetRecentPostsByUserId(_userResolver.UserId);

            // Step 1: Identify the most common topic among userRecentPosts
            var mostCommonTopic = userRecentPosts
                .GroupBy(post => post.Category)
                .OrderByDescending(group => group.Count())
                .First().Key;

            // Get recently posted posts with most common topic for user
            var recentPosts = await _recommendationServiceDataAccess.GetRecentPostsByTopic(_userResolver.UserId);

            // Step 2: Filter recentPosts by the most common topic
            var postsWithCommonTopic = recentPosts.Where(post => post.Category == mostCommonTopic);

            // Define weights for views, comments, and likes
            var weights = new Dictionary<string, double>
            {
                { "views_count", 0.46 },
                { "comments_count", 0.27 },
                { "likes", 0.27 }
            };

            // Calculate popularity score for recently posted posts
            var popularityScores = postsWithCommonTopic.Select(post =>
                post.ViewsCount * weights["views_count"] +
                //post.CommentsCount * weights["comments_count"] +
                post.LikesCount * weights["likes"]);

            // Recommend the most popular post
            var mostPopularPost = postsWithCommonTopic
                .OrderByDescending(post => popularityScores)
                .First();

            return mostPopularPost;
        }
    }
}
