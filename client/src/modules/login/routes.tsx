import { RouteObject } from "react-router-dom";
import { LoginPage } from "./components";

const ROUTES: RouteObject[] = [
	{
		path: '/login',
		element: <LoginPage />,
	}
]

export default ROUTES;