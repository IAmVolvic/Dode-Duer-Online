using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController(IPriceService priceService) : ControllerBase
{
    [HttpGet]
    [Route("GetPrices")]
    public ActionResult<List<PriceDto>> GetPrices()
    {
        var prices = priceService.GetPrices();
        return Ok(prices);
    }
}