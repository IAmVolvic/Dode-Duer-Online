import { RouteObject } from "react-router-dom";

import HomeRoutes from "@modules/home/routes";
import LoginRoutes from "@modules/login/routes";


const ROUTES: RouteObject[] = [
	...HomeRoutes,
	...LoginRoutes
]

export default ROUTES;