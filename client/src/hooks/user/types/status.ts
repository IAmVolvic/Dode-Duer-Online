export enum TransactionStatus {
    Pending     = 'Pending',
    Approved    = 'Approved',
    Rejected    = 'Rejected'
};

export const transactionStatusColor: { [key in TransactionStatus]: {textColor, textContent, backgroundContent, background} } = {
    [TransactionStatus.Pending]:   {
        textColor: "text-warning",
        textContent: "text-warning-content",
        backgroundContent: "bg-warning-content",
        background: "bg-warning"
    },
    [TransactionStatus.Approved]:  {
        textColor: "text-success",
        textContent: "text-success-content",
        backgroundContent: "bg-success-content",
        background: "bg-success"
    },
    [TransactionStatus.Rejected]:  {
        textColor: "text-error",
        textContent: "text-error-content",
        backgroundContent: "bg-error-content",
        background: "bg-error"
    }
};