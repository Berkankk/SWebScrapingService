using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWebScrapingService.DTOS;
using SWebScrapingService.Service;

namespace SWebScrapingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScrapingController : ControllerBase //Bu controller atılan istekleri(http) karşılamak için var 
    {
        private readonly MongoService _mongoService; //Mongonun da haberi olsun 
        private readonly ScrapingService _scrapingService; //Kazıma işlemi yapmak için kullanılır 
        public ScrapingController(MongoService mongoService, ScrapingService scrapingService)
        {
            _mongoService = mongoService;
            _scrapingService = scrapingService;
        }

        [HttpPost]
        public async Task<IActionResult> ScrapeAndSave([FromBody] ScrapingRequestDto request) //Url bilgisini alarak yola çıktık
        {
            if (string.IsNullOrEmpty(request.Url))
            {
                return BadRequest(new { Message = "Url boş olamaz" });
            }

            var headlines = await _scrapingService.ScrapeHeadlinesAsync(request.Url);

            if (headlines.Count == 0)
            {
                return NotFound(new { Message = "Verilen url de başlık yok" });
            }

            await _mongoService.SaveHeadlinesAsync(headlines);
            return Ok(new { Message = "Tebrikler kazıyıp attınız", Headlines = headlines });
        }




    }
}
