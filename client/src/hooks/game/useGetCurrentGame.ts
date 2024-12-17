import { Api, GameResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetCurrentGame = () => {
    return useQuery({
        queryKey: ['game-current'],

        queryFn: async (): Promise<GameResponseDTO[]> => {
            return API.game.gameGetAllGames().then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 1000,
    });
}