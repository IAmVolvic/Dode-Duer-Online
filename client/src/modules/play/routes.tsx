import { RouteObject } from "react-router-dom";
import { RootOutlet } from "@app/components/outlet";
import {PlayPage} from "./components/Play";

const ROUTES: RouteObject[] = [
    {
        path: '/play',
        element: <RootOutlet isProtected={true} failedAuthPath={"/login"}/>,
        children: [
            { index: true, element: <PlayPage/> },
        ]
    }
]

export default ROUTES;