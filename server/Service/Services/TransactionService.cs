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
            /*UserId = user.Id,*/
            User = user,
            /*TransactionNumber = depositRequest.TransactionNumber,
            TransactionPhoneNumber = depositRequest.TransactionPhoneNumber,
            TransactionUsername = depositRequest.TransactionUsername,
            TransactionStatus = "Pending"*/
        };

        if (TransactionNumberExists(depositRequest.TransactionNumber))
        {
            throw new ErrorException("Transaction", "Transaction already exists");
        }
        
        transactionRepository.NewTransaction(transaction);
        return TransactionResponseDTO.FromEntity(transaction);
    }


    public decimal TransactionAdjustment(BalanceAdjustmentRequestDTO balanceAdjustmentRequest)
    {
        var transaction = transactionRepository.GetTransactionById(balanceAdjustmentRequest.TransactionId);
        var userBalance = transaction.User.Balance;
        
        /*if (transaction == null)
        {
            throw new ErrorException("Transaction", "Transaction not found");
        }
        
        // Adjust the new balance
        if (balanceAdjustmentRequest.Adjustment == BalanceAdjustmentRequestDTO.AdjustmentType.Deduct)
        {
            userBalance -= balanceAdjustmentRequest.Amount;
            userBalance = Math.Max(userBalance, 0);
        }
        
        if (balanceAdjustmentRequest.Adjustment == BalanceAdjustmentRequestDTO.AdjustmentType.Deposit)
        {
            userBalance += balanceAdjustmentRequest.Amount;
            userBalance = Math.Min(userBalance, 50000);
        }

        var updatedUser = new User
        {
            Id = transaction.User.Id,
            Balance = userBalance
        };
        
        userRepository.UpdateUserDB(updatedUser);
        return userBalance;*/
        return 0;
    }


    public TransactionResponseDTO[] Transactions()
    {
        var transactionList = new List<TransactionResponseDTO>();
        var transactions = transactionRepository.GetAllTransactions();
        
        foreach (var transaction in transactions)
        {
            var transactionDTO = TransactionResponseDTO.FromEntity(transaction);
            transactionList.Add(transactionDTO);
        }
        
        return transactionList.ToArray();
    }
    
    
    public TransactionResponseDTO[] TransactionsByUser(Guid userId)
    {
        var transactionList = new List<TransactionResponseDTO>();
        Console.WriteLine("Am I here?");
        var transactions = transactionRepository.GetAllTransactionsByUserId(userId);

        if (transactions == null)
        {
            throw new ErrorException("Transactions", "There are multiple transactions for this user");
        }
        
        foreach (var transaction in transactions)
        {
            var transactionDTO = TransactionResponseDTO.FromEntity(transaction);
            transactionList.Add(transactionDTO);
        }
        
        return transactionList.ToArray();
    }
    
    
    private bool TransactionNumberExists(string transactionNumber)
    {
        return transactionRepository.TransactionAlreadyExists(transactionNumber);
    }
}