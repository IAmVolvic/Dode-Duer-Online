using API.Attributes;
using Microsoft.AspNetCore.Mvc;
using Service.TransferModels.Requests;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController: ControllerBase
{
    
    [HttpPatch]
    [Route("@user/balance/deposit")]
    [Authenticated]
    public ActionResult<Boolean> PDepositBalance([FromBody] DepositRequestDTO data)
    {
        return Ok(true);
    }
    
    
    [HttpPatch]
    [Route("@user/balance")]
    [Rolepolicy("Admin")]
    public ActionResult<Boolean> PUseBalance([FromBody] DepositRequestDTO data)
    {
        return Ok(true);
    }
    
    
    [HttpGet]
    [Route("@user/balances")]
    [Rolepolicy("Admin")]
    public ActionResult<Boolean> PUserDeposits([FromBody] DepositRequestDTO data)
    {
        return Ok(true);
    }
    
}