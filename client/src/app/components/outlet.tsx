import { Outlet } from "react-router-dom"
import { ProtectedComponent } from "@components/authProtected/ProtectedComponent"
import { Toaster } from "react-hot-toast";
import ThemeSwitcher from "@components/themeSwitcher";
import { TopNavigation } from "@components/navigation";
import { Background } from "@components/background";
import { Footer } from "@components/footer";


interface RouteOutletProps {
	isProtected: boolean;
	failedAuthPath?: string;
}


const RootContent = () => {
	return (
		<>
			<Background />
			<TopNavigation />
			<Toaster position="top-center"/>
			<ThemeSwitcher />
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