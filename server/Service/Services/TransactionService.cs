using API.Exceptions;
using DataAccess.Interfaces;
using DataAccess.Models;
using Service.Services.Interfaces;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;

namespace Service.Services;

public class TransactionService(IUserRepository userRepository, ITransactionRepository transactionRepository) : ITransactionService
{
    public TransactionResponseDTO NewTransactionRequest(Guid userId, DepositRequestDTO depositRequest)
    {
        Guid transactionId = Guid.NewGuid();
        var user = userRepository.GetUserById(userId.ToString());

        var transaction = new Transaction
        {
            Id = transactionId,
            Userid = user.Id,
            User = user,
            Transactionnumber = depositRequest.TransactionNumber
        };

        if (TransactionNumberExists(depositRequest.TransactionNumber))
        {
            throw new ErrorException("Transaction", "Transaction already exists");
        }
        
        transactionRepository.NewTransaction(transaction);
        return TransactionResponseDTO.FromEntity(transaction, user);
    }


    public decimal TransactionAdjustment(BalanceAdjustmentRequestDTO balanceAdjustmentRequest)
    {
        var transaction = transactionRepository.GetTransactionById(balanceAdjustmentRequest.TransactionId);
        var user = transaction.User;
        var userBalance = user.Balance;
        
        if (transaction == null)
        {
            throw new ErrorException("Transaction", "Transaction not found");
        }
        
        // Adjust the new balance
        if (balanceAdjustmentRequest.Adjustment == DataAccess.Types.Enums.TransactionAdjustment.Deduct)
        {
            userBalance -= balanceAdjustmentRequest.Amount;
            userBalance = Math.Max(userBalance, 0);
        }
        
        if (balanceAdjustmentRequest.Adjustment == DataAccess.Types.Enums.TransactionAdjustment.Deposit)
        {
            userBalance += balanceAdjustmentRequest.Amount;
            userBalance = Math.Min(userBalance, 50000);
        }
        
        user.Balance = userBalance;
        userRepository.UpdateUserDb(user);
        transactionRepository.UpdateTransactionStatus(transaction.Id, balanceAdjustmentRequest.TransactionStatusA);
        
        return userBalance;
    }


    public TransactionResponseDTO[] Transactions()
    {
        var transactionList = new List<TransactionResponseDTO>();
        var transactions = transactionRepository.GetAllTransactions();
        
        foreach (var transaction in transactions)
        {
            var transactionDTO = TransactionResponseDTO.FromEntity(transaction, transaction.User);
            transactionList.Add(transactionDTO);
        }
        
        return transactionList.ToArray();
    }
    
    
    public TransactionResponseDTO[] TransactionsByUser(Guid userId)
    {
        var transactionList = new List<TransactionResponseDTO>();
        var transactions = transactionRepository.GetAllTransactionsByUserId(userId);

        if (transactions == null)
        {
            throw new ErrorException("Transactions", "There are multiple transactions for this user");
        }
        
        foreach (var transaction in transactions)
        {
            var transactionDTO = TransactionResponseDTO.FromEntity(transaction, transaction.User);
            transactionList.Add(transactionDTO);
        }
        
        return transactionList.ToArray();
    }
    
    
    private bool TransactionNumberExists(string transactionNumber)
    {
        return transactionRepository.TransactionAlreadyExists(transactionNumber);
    }
}