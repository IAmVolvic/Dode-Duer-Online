import { Api, WinnersDto } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetWinnersOfGame = (gameId: string) => {
    return useQuery({
        queryKey: ['game-winners'],
        queryFn: async (): Promise<WinnersDto[]> => {
            return API.winners.winnersGetWinners(gameId).then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 10000,
    });
}