using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using RecommendationManagementService.Core;
using RecommendationManagementService.Core.AppSettings;
using RecommendationManagementService.Core.Enum;
using RecommendationManagementService.Core.Model;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Data.Implementation
{
    public class UserInteractionDataAccess : BaseServiceDataAccess, IUserInteractionDataAccess
    {
        protected readonly IMongoCollection<UserInteraction> _userInteractionCollection;

        public UserInteractionDataAccess(IOptions<MongoDbSettings> mongoDbSettings) : base(mongoDbSettings)
        {
            _userInteractionCollection = _mongoDatabase.GetCollection<UserInteraction>(mongoDbSettings.Value.UserInteractionCollectionName);
        }

        public async Task<List<UserInteraction>> FindUserInteraction(string postId, string userId)
        {
            var filter = Builders<UserInteraction>.Filter.And(
                Builders<UserInteraction>.Filter.Eq(userInteraction => userInteraction.PostId, postId),
                Builders<UserInteraction>.Filter.Eq(userInteraction => userInteraction.UserId, userId)
                );

            var result = await _userInteractionCollection.FindAsync<UserInteraction>(filter);

            return result.ToList();
        }

        public async Task<List<UserInteraction>> GetAllUserInteractions(string userId)
        {
            var filter = Builders<UserInteraction>.Filter.Eq(userInteraction => userInteraction.UserId, userId);

            var result = await _userInteractionCollection.FindAsync<UserInteraction>(filter);

            return result.ToList();
        }

        public async Task<string> GetMostPopularCategoryForUser(string userId)
        {
            var query = _userInteractionCollection.AsQueryable()
                .Where(x => x.UserId == userId)
                .GroupBy(x => x.Category)
                .Select(group => new
                {
                    Category = group.Key,
                    Rating = group.Sum(RecommendationHelper.RatingSelector())
                })
                .OrderByDescending(x => x.Rating)
                .Take(1);

            var result = query.FirstOrDefault();

            return result.Category;
        }

        public async Task<List<string>> GetMostPopularPostsForCategory(string category)
        {
            var query = _userInteractionCollection.AsQueryable()
                .Where(x => x.Category == category)
                .GroupBy(x => x.PostId)
                .Select(group => new 
                {
                    PostId = group.Key,
                    Rating = group.Sum(RecommendationHelper.RatingSelector())
                })
                .OrderByDescending(x => x.Rating)
                .Take(5);

            var result = query.Select(x => x.PostId).ToList();

            return result;
        }

        public async Task<List<string>> GetPostsForCategoryUserHasInteractedWith(string category, string userId)
        {
            var query = _userInteractionCollection.AsQueryable()
                .Where(x => x.Category == category && x.UserId == userId)
                .Select(x => x.PostId);

            var result = query.ToList();

            return result;
        }

        public async Task SaveUserInteraction(UserInteraction userInteraction)
        {
            await _userInteractionCollection.InsertOneAsync(userInteraction);
        }

        public async Task SaveUserInteractions(List<UserInteraction> userInteractions)
        {
            await _userInteractionCollection.InsertManyAsync(userInteractions);
        }
    }
}
