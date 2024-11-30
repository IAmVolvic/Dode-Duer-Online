import { Outlet } from "react-router-dom"
import { ProtectedComponent } from "@components/authProtected/ProtectedComponent"
import { Toaster } from "react-hot-toast";
import ThemeSwitcher from "@components/themeSwitcher";


interface RouteOutletProps {
	isProtected: boolean;
	failedAuthPath?: string;
}


const RootContent = () => {
	return (
		<>
			<Toaster position="top-center"/>
			<Outlet />
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