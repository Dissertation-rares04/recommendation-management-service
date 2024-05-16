using RecommendationManagementService.Core.Enum;

namespace RecommendationManagementService.Core.Model
{
    public class UserInteraction
    {
        public string UserId { get; set; }

        public string PostId { get; set; }

        public string Category { get; set; }

        public long Timestamp { get; set; }

        public ActionType Action { get; set; }
    }
}
