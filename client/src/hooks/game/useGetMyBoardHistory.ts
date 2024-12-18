import { Api, MyBoards } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetMyBoardHistory = () => {
    return useQuery({
        queryKey: ['my-board-history'],
        queryFn: async (): Promise<MyBoards[]> => {
            return API.board.boardUserBoardHistory().then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 10000,
    });
}