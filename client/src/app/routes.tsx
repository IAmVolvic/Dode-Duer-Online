import { RouteObject } from "react-router-dom";

import HomeRoutes from "@modules/home/routes";
import LoginRoutes from "@modules/login/routes";
import ErrorsRoutes from "@modules/errors/routes";
import PlayRoutes from "@modules/play/routes";

import WNumbersRoutes from "modules/winNumbers/routes"



const ROUTES: RouteObject[] = [
	...HomeRoutes,
	...LoginRoutes,
	...PlayRoutes,


	...ErrorsRoutes,

	...WNumbersRoutes

]

export default ROUTES;