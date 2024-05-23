using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace RecommendationManagementService.Core.Model
{
    public class UserRecommendation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string PostId { get; set; }
    }
}
