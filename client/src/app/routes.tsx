import { RouteObject } from "react-router-dom";

import HomeRoutes from "@modules/home/routes";
import LoginRoutes from "@modules/login/routes";
import ErrorsRoutes from "@modules/errors/routes";


const ROUTES: RouteObject[] = [
	...HomeRoutes,
	...LoginRoutes,


	...ErrorsRoutes
]

export default ROUTES;