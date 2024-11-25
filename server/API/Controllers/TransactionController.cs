using API.Attributes;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(ITransactionService service): ControllerBase
{
    
    [HttpPost]
    [Route("@user/balance/deposit")]
    [Authenticated]
    public ActionResult<TransactionResponseDTO> PDepositReq([FromBody] DepositRequestDTO data)
    {
        var authUser = HttpContext.Items["AuthenticatedUser"] as AuthorizedUserResponseDTO;
        return Ok(service.NewTransactionRequest(authUser!.Id, data));
    }
    
    
    [HttpPatch]
    [Route("@admin/balance")]
    [Rolepolicy("Admin")]
    public ActionResult<Boolean> PUseBalance([FromBody] BalanceAdjustmentRequestDTO data)
    {
        return Ok(service.TransactionAdjustment(data));
    }
    
    
    [HttpGet]
    [Route("@admin/balances")]
    [Rolepolicy("Admin")]
    public ActionResult<TransactionResponseDTO[]> PDepositReqs()
    {
        return Ok(service.Transactions());
    }
    
}