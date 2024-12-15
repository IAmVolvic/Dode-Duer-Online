import { Api, AuthorizedUserResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query"

const API = new Api();

export const useGetAllUsers = () => {
    return useQuery({
        queryKey: ['user-all'],
        queryFn: async (): Promise<AuthorizedUserResponseDTO[]> => {
            return API.user.userGGetUsers().then((res) => res.data);
        },

        refetchOnMount: true,
        refetchInterval: 10000,
    });
}