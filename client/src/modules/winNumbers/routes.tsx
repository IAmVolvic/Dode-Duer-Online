import { RouteObject } from "react-router-dom";
import { WinningNumbers } from "./components/SetWinningNumbers";

const ROUTES: RouteObject[] = [
    {
        path: '/WNumbers',
        element: <WinningNumbers />,
    }
]

export default ROUTES;