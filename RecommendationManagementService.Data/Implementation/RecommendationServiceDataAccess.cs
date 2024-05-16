using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RecommendationManagementService.Core.AppSettings;
using RecommendationManagementService.Core.Model;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Data.Implementation
{
    public class RecommendationServiceDataAccess : BaseServiceDataAccess, IRecommendationServiceDataAccess
    {
        protected readonly IMongoCollection<Post> _postCollection;
        protected readonly IMongoCollection<Post> _commentsCollection;
        protected readonly IMongoCollection<UserRecommendation> _userRecommendationsCollection;

        public RecommendationServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
            _postCollection = _mongoDatabase.GetCollection<Post>(mongoDbSettings.Value.PostCollectionName);
            _commentsCollection = _mongoDatabase.GetCollection<Post>(mongoDbSettings.Value.CommentCollectionName);
            _userRecommendationsCollection = _mongoDatabase.GetCollection<UserRecommendation>(mongoDbSettings.Value.UserRecommendationCollectionName);
        }

        public async Task<List<Post>> GetLatestPostsByCategory(string category)
        {
            var filter = Builders<Post>.Filter.And(
                Builders<Post>.Filter.Gte(post => post.CreatedAt, DateTime.UtcNow.AddDays(-7)),
                Builders<Post>.Filter.Eq(post => post.Category, category)
                );

            var recentPosts = await _postCollection.FindAsync<Post>(filter);

            return recentPosts.ToList();
        }

        public async Task<List<Post>> GetRecentPostsByUserId(string userId)
        {
            var filter = Builders<Post>.Filter.And(
                Builders<Post>.Filter.Gte(post => post.CreatedAt, DateTime.UtcNow.AddDays(-7)),
                Builders<Post>.Filter.Where(post =>
                    post.Likes.Any(x => x.UserId == userId && x.CreatedAt >= DateTime.UtcNow.AddDays(-7)) ||
                    post.Views.Any(x => x.UserId == userId && x.CreatedAt >= DateTime.UtcNow.AddDays(-7)))
                );

            var recentPosts = await _postCollection.FindAsync<Post>(filter);

            return recentPosts.ToList();
        }

        public async Task<List<Post>> GetRecentPostsByTopic(string topic)
        {
            var filter = Builders<Post>.Filter.And(
                Builders<Post>.Filter.Gte(post => post.CreatedAt, DateTime.UtcNow.AddDays(-7)),
                Builders<Post>.Filter.Eq(post => post.Category, topic)
                );

            var recentPosts = await _postCollection.FindAsync<Post>(filter);

            return recentPosts.ToList();
        }

        public async Task SaveUserRecommendations(List<UserRecommendation> userRecommendations)
        {
            await _userRecommendationsCollection.InsertManyAsync(userRecommendations);
        }
    }
}
