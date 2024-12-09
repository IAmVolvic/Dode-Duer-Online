import { Api, TransactionResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetUsersTransactions = () => {
    return useQuery({
        queryKey: ['users-transactions'],
        queryFn: async (): Promise<TransactionResponseDTO[]> => {
            return API.transaction.transactionPDepositReqs().then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 10000,
    });
}