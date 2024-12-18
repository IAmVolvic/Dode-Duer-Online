import { Api, WinnersDto } from "@Api";
import { useQuery } from "@tanstack/react-query";

const API = new Api();

export const useGetWinnersOfGame = (gameId: string) => {
    return useQuery({
        queryKey: ['game-winners', gameId], // Add gameId to query key to refetch on gameId change
        queryFn: async (): Promise<WinnersDto[]> => {
            if (!gameId) {
                return []; // Return an empty array if no gameId is provided
            }
            return API.winners.winnersGetWinners(gameId).then((res) => res.data);
        },

        enabled: !!gameId,
        refetchOnMount: true,
        refetchInterval: 10000,
    });
};