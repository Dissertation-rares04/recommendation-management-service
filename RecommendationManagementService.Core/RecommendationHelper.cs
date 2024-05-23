using RecommendationManagementService.Core.Enum;
using RecommendationManagementService.Core.Model;

namespace RecommendationManagementService.Core
{
    public static class RecommendationHelper
    {
        public static Func<UserInteraction, int> RatingSelector()
        {
            static int func(UserInteraction x)
            {
                return x.InteractionType switch
                {
                    InteractionType.VIEW => 1,
                    InteractionType.LIKE => 2,
                    _ => 0
                };
            }

            return func;
        }
    }
}
