using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Types.Enums;

namespace DataAccess.Repositories;

public class TransactionRepository(LotteryContext context) : ITransactionRepository
{

    public Transaction NewTransaction(Transaction newTransaction)
    {
        context.Transactions.Add(newTransaction);
        context.SaveChanges();
        return newTransaction;
    }


    public Transaction GetTransactionById(string transactionId)
    {
        var data = context.Transactions.FirstOrDefault(u => u.Id == Guid.Parse(transactionId));
        if(data == null)
        {
            Console.WriteLine($"Transaction with id {transactionId} not found");
            return null;
        }
        
        return data;
    }


    public Transaction[] GetAllTransactions()
    {
        return context.Transactions
            .OrderByDescending(t => (int)t.Transactionstatus == (int)TransactionStatusA.Pending)
            .ThenBy(t => t.Id)
            .ToArray();
    }


    public Transaction[] GetAllTransactionsByUserId(Guid userId)
    {
        var transactions = context.Transactions
            .Where(t => t.Userid == userId)
            .OrderByDescending(t => (int)t.Transactionstatus == (int)TransactionStatusA.Pending)
            .ThenBy(t => t.Id)
            .ToArray();

        if (transactions.Length == 0)
        {
            return null;
        }


        return transactions;
    }
    
    
    public Boolean TransactionAlreadyExists(string transactionNumber)
    {
        return context.Transactions.Any(t => t.Transactionnumber == transactionNumber);
    }
    
    public Transaction UpdateTransactionStatus(Guid transactionId, TransactionStatusA newStatus)
    {
        var updatedTransaction = context.Transactions.Find(transactionId);
        updatedTransaction.Transactionstatus = newStatus;
        
        context.SaveChanges();
        return updatedTransaction;
    }
}