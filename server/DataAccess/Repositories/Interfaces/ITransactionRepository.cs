using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface ITransactionRepository
{
    public Transaction NewTransaction(Transaction newTransaction);

    public Boolean TransactionAlreadyExists(string transactionNumber);

    public Transaction GetTransactionById(string transactionId);

    public Transaction[] GetAllTransactions();
    
    public Transaction[] GetAllTransactionsByUserId(Guid userId);
}