import { Outlet } from "react-router-dom"
import { ProtectedComponent } from "@components/authProtected/ProtectedComponent"
import { Toaster } from "react-hot-toast";
import { TopNavigation } from "@components/navigation";
import { Background } from "@components/background";
import { Footer } from "@components/footer";

import { ThemeAtom } from "@atoms/ThemeAtom";
import { useAtom } from "jotai";
import { useEffect } from "react";


interface RouteOutletProps {
	isProtected: boolean;
	failedAuthPath?: string;
}


const RootContent = () => {
    const [theme] = useAtom(ThemeAtom);

	useEffect(() => {
		const theme = localStorage.getItem('theme') as string;
		document.documentElement.setAttribute('data-theme', theme);
	}, [theme]);

	return (
		<>
			<Background />
			<TopNavigation />
			<Toaster position="top-center"/>
			<Outlet />
			<Footer />
		</>
	)
}


export const RootOutlet = (props: RouteOutletProps) => {
	return props.isProtected ? (
		<ProtectedComponent showWhileAuthenticated={true} redirect={props.failedAuthPath!}>
			<RootContent />
		</ProtectedComponent>
	) : (
		<RootContent />
	)
}