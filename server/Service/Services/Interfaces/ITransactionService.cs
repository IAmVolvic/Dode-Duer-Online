using DataAccess.Models;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services.Interfaces;

public interface ITransactionService
{
    public TransactionResponseDTO NewTransactionRequest(Guid userId, DepositRequestDTO depositRequest);

    public decimal TransactionAdjustment(BalanceAdjustmentRequestDTO balanceAdjustmentRequest);

    public TransactionResponseDTO[] Transactions();

    public TransactionResponseDTO[] TransactionsByUser(Guid userId);
}