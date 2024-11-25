using DataAccess.Contexts;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess.Repositories;

public class TransactionRepository(UserContext context) : ITransactionRepository
{

    public Transaction NewTransaction(Transaction newTransaction)
    {
        context.Transactions.Add(newTransaction);
        context.SaveChanges();
        return newTransaction;
    }


    public Transaction GetTransactionById(string transactionId)
    {
        if(context.Transactions.Any(t => t.Id != Guid.Parse(transactionId)))
        {
            return null;
        }
        
        return context.Transactions.FirstOrDefault(u => u.Id == Guid.Parse(transactionId));
    }


    public Transaction[] GetAllTransactions()
    {
        return context.Transactions
            .OrderByDescending(t => t.TransactionStatus == "Pending")
            .ThenBy(t => t.Id) 
            .ToArray();
    }


    public Transaction[] GetAllTransactionsByUserId(Guid userId)
    {
        var transactions = context.Transactions
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.TransactionStatus == "Pending")
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
        return context.Transactions.Any(t => t.TransactionNumber == transactionNumber);
    }
}