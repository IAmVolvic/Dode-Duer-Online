using DataAccess.Models;
using DataAccess.Types.Enums;

namespace DataAccess.Interfaces;

public interface ITransactionRepository
{
    public Transaction NewTransaction(Transaction newTransaction);

    public Boolean TransactionAlreadyExists(string transactionNumber);

    public Transaction GetTransactionById(string transactionId);

    public Transaction[] GetAllTransactions();
    
    public Transaction[] GetAllTransactionsByUserId(Guid userId);
    
    public Transaction UpdateTransactionStatus(Guid transactionId, TransactionStatusA newStatus);
}