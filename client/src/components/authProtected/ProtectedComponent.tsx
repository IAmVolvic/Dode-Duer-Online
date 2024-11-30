import { Navigate, To } from "react-router-dom";

import { useAuth } from "@hooks/authentication/useAuthentication";

interface ProtectedComponentProps {
	showWhileAuthenticated: boolean;
	redirect: To;

	children: React.ReactNode;
}

export const ProtectedComponent = (props: ProtectedComponentProps) => {
	const { showWhileAuthenticated, redirect, children } = props;
	const { user, isLoggedIn } = useAuth();

	if (showWhileAuthenticated) {
		if (!isLoggedIn) {
			return <Navigate to={redirect} replace={true} />
		}
	} else {
		if (user) {
			return <Navigate to={redirect} replace={true} />
		}
	}

	return children;
}