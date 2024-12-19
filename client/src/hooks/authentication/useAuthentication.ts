import { Signal } from "@preact/signals-react";
import { useEffect, useState } from "react";
import { Api, AuthorizedUserResponseDTO } from "@Api";
import { useQuery } from "@tanstack/react-query";


const API = new Api();
const localUser = window.localStorage.getItem("user");
const userSignal = new Signal<AuthorizedUserResponseDTO | undefined>( localUser ? JSON.parse(localUser) : undefined );
const loggedIn = new Signal<boolean>(!!localUser);
export const bToken = document.cookie.split('; ').find(row => row.startsWith('Authentication='))?.split('=')[1] || undefined;

const updateCache = (user: AuthorizedUserResponseDTO) => {
    window.localStorage.setItem("user", JSON.stringify(user));
    userSignal.value = user;
    loggedIn.value = true;
};


export const setAuth = () => {
    return API.user.userGGetUser().then((res) => {
        if (!res.data || loggedIn.value) {
            clearAuth();
        } else {
            updateCache(res.data);
            window.location.href = '/';
        }
    });
};


export const clearAuth = () => {
    window.localStorage.removeItem("user");
    loggedIn.value = false;
    userSignal.value = undefined;
    window.location.href = '/';
};


export const useAuth = () => {
    const [user, setUser] = useState(userSignal.value);
    const [isLoggedIn, setIsLoggedIn] = useState(loggedIn.value);

    useEffect(() => {
        const userUnsub = userSignal.subscribe((value) => setUser(value));
        const loggedInUnsub = loggedIn.subscribe((value) => setIsLoggedIn(value));

        return () => {
            userUnsub();
            loggedInUnsub();
        };
    }, []);

    const useQ = useQuery({
        queryKey: ["user-data"],
        queryFn: async (): Promise<AuthorizedUserResponseDTO> => {
            const response = await API.user.userGGetUser();
            updateCache(response.data);
            return response.data;
        },
        retry: false,

        enabled: (bToken !== undefined)? true : false,

        refetchInterval: 10 * 3000,
        refetchOnWindowFocus: true,
        refetchOnMount: false,
        refetchOnReconnect: true,
    });

    return { user, isLoggedIn, refresh: useQ.refetch };
};