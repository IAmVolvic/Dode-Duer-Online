import { Signal } from "@preact/signals-react";
import { useEffect, useState } from "react";
import { Api, AuthorizedUserResponseDTO } from "@Api";


const API = new Api();
export const localUser = window.localStorage.getItem("user")
const userSignal = new Signal<AuthorizedUserResponseDTO | undefined>(localUser ? JSON.parse(localUser) : undefined)
const loggedIn = new Signal<boolean>(!!localUser);


export const setAuth = () => {
    const API = new Api();
    return API.user.userGGetUser().then((res) => {
        if (res.data || loggedIn.value === true) {
            window.localStorage.removeItem("user")
            loggedIn.value = false
            userSignal.value = undefined
        }
        
        window.localStorage.setItem("user", JSON.stringify(res.data))
        userSignal.value = res.data
        loggedIn.value = true

        window.location.href = '/';
    })
};


export const clearAuth = () => {
	window.localStorage.removeItem("user")
	loggedIn.value = false
	userSignal.value = undefined
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

    return { user, isLoggedIn };
};