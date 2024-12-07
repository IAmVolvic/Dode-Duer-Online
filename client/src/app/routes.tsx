import { RouteObject } from "react-router-dom";

import HomeRoutes from "@modules/home/routes";
import LoginRoutes from "@modules/login/routes";
import ErrorsRoutes from "@modules/errors/routes";

import WNumbersRoutes from "modules/winNumbers/routes"



const ROUTES: RouteObject[] = [
	...HomeRoutes,
	...LoginRoutes,



	...ErrorsRoutes,

	...WNumbersRoutes

]

export default ROUTES;