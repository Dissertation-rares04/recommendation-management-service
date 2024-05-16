namespace RecommendationManagementService.Core.AppSettings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string PostCollectionName { get; set; }
        public string CommentCollectionName { get; set; }
        public string UserInteractionCollectionName { get; set; }
        public string UserRecommendationCollectionName { get; set; }
    }
}
