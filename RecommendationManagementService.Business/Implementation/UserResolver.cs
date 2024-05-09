using Microsoft.AspNetCore.Http;
using RecommendationManagementService.Business.Interface;

namespace RecommendationManagementService.Business.Implementation
{
    public class UserResolver : IUserResolver
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public UserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor
                .HttpContext?
                .Items["UserId"] as string
                ?? throw new Exception();
        }

        public string UserId => _userId;
    }
}
