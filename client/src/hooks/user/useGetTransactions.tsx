import { Api, TransactionResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetTransactions = () => {
    return useQuery({
        queryKey: ['user-transactions'],
        queryFn: async (): Promise<TransactionResponseDTO[]> => {
            return API.transaction.transactionPUserTransactionsReqs().then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 10000,
    });
}