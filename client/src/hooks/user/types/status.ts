export enum TransactionStatus {
    Pending     = 'Pending',
    Approved    = 'Approved',
    Rejected    = 'Rejected'
};

export const transactionStatusColor: { [key in TransactionStatus]: {textColor, textContent, backgroundContent, background} } = {
    [TransactionStatus.Pending]:   {
        textColor: "text-warning",
        textContent: "text-warning-content",
        backgroundContent: "bg-warning-content/30",
        background: "bg-warning"
    },
    [TransactionStatus.Approved]:  {
        textColor: "text-success",
        textContent: "text-success-content",
        backgroundContent: "bg-success-content/30",
        background: "bg-success"
    },
    [TransactionStatus.Rejected]:  {
        textColor: "text-error",
        textContent: "text-error-content",
        backgroundContent: "bg-error-content/30",
        background: "bg-error"
    }
};