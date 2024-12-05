import { Api, TransactionResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetTransactions = () => {
    return useQuery({
        queryKey: ['user-transactions'],
        queryFn: async (): Promise<TransactionResponseDTO[]> => {
            const response = await API.transaction.transactionPUserTransactionsReqs();
            return response.data;
        },


        refetchOnMount: true,
        refetchInterval: 10000,
    });
}