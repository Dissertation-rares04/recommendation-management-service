using Microsoft.AspNetCore.Mvc;
using RecommendationManagementService.Business.Interface;

namespace RecommendationManagementService.API.Controllers
{
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpGet("daily-recommendation")]
        public async Task<IActionResult> GetDailyRecommendation()
        {
            try
            {
                await _recommendationService.GetDailyRecommendation();

                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
