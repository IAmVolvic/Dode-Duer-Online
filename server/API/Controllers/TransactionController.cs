using API.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/billing")]
public class TransactionController : ControllerBase
{
    
    [HttpGet]
    [Route("balance")]
    [Authenticated]
    public ActionResult<Boolean> CheckBalance()
    {
        return Ok(true);
    }
    
    
    [HttpPost]
    [Route("balance/deposit")]
    [Authenticated]
    public ActionResult<Boolean> DepositBalance()
    {
        return Ok(true);
    }
    
    
    [HttpPost]
    [Route("balance/purchase-board")]
    [Authenticated]
    public ActionResult<Boolean> PurchaseBoard()
    {
        return Ok(true);
    }
    
}