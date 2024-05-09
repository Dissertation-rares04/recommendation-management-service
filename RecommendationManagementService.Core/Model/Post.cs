using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RecommendationManagementService.Core.Model
{
    public class Post : AuditableEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string Topic { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<View> Views { get; set; }
        public int ViewsCount { get; set; }

        public List<Like> Likes { get; set; }
        public int LikesCount { get; set; }

        public List<Dislike> Dislikes { get; set; }
        public int DislikesCount { get; set; }

        public List<Media> Medias { get; set; }
    }
}
