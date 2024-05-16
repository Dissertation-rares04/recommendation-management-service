using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RecommendationManagementService.Core.AppSettings;
using RecommendationManagementService.Data.Interface;

namespace RecommendationManagementService.Data.Implementation
{
    public class BaseServiceDataAccess : IBaseServiceDataAccess
    {
        protected readonly IMongoDatabase _mongoDatabase;

        public BaseServiceDataAccess(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);

            _mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        }
    }
}
