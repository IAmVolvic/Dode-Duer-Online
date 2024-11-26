import { useQuery } from "@tanstack/react-query"
import axios from "axios"
import { Signal } from "@preact/signals-react";
import { useMemo } from "react";

export interface AuthUser {
	id: number,
	username: string;
	avatar: string;
	permissions: string[];
	isAdmin: boolean;
}

const localUser = window.localStorage.getItem("user")
const userSignal = new Signal<AuthUser | undefined>(localUser ? JSON.parse(localUser) : undefined)
const loggedIn = new Signal<boolean>(!!localUser);

export const useAuth = () => {
	useQuery({
		queryKey: ["auth"],
		queryFn: async (): Promise<AuthUser> => {
			return axios.request({
				url: "",
				withCredentials: true,
				method: "GET",
			})
			.then((res) => {
				if (res.data.status && res.data.message) {
					window.localStorage.removeItem("user")
					loggedIn.value = false
					userSignal.value = undefined
					throw new Error(res.data.message)
				}
				
				window.localStorage.setItem("user", JSON.stringify(res.data))
				userSignal.value = res.data
				loggedIn.value = true
				return res.data
			})
		},
		
		retry: false,
		
		refetchInterval: 30 * 1000,
		refetchOnWindowFocus: true,
		refetchOnMount: false,
		refetchOnReconnect: true
	})

	return useMemo(() => ({ user: userSignal, isLoggedIn: loggedIn }), [])
}