using RecommendationManagementService.Business.Interface;

namespace RecommendationManagementService.Business.Implementation
{
    public class BaseService : IBaseService
    {
        protected readonly IUserResolver _userResolver;

        public BaseService(IUserResolver userResolver)
        {
            _userResolver = userResolver;
        }
    }
}
