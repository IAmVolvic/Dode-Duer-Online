import { RouteObject } from "react-router-dom";
import { Login } from "./components";

const ROUTES: RouteObject[] = [
	{
		path: '/login',
		element: <Login />,
	}
]

export default ROUTES;