using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecommendationManagementService.Core.Model
{
    public class Comment : AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PostId { get; set; }

        public string UserId { get; set; }

        public string Content { get; set; }

        public List<Like> Likes { get; set; }
    }
}
