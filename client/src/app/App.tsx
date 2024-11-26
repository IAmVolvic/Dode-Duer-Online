import ROUTES from "./routes"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { createBrowserRouter, RouterProvider } from "react-router-dom"
import {Toaster} from "react-hot-toast";

import {initOrdersAtom} from "@atoms/AllOrdersAtom.ts";
import {initPropertiesAtom} from "@atoms/PropertiesAtom.ts";


export const QUERY_CLIENT = new QueryClient();


const App = () => {
	const router = createBrowserRouter(ROUTES)

    initOrdersAtom();

    initPropertiesAtom();

    return (
        <>
            <QueryClientProvider client={QUERY_CLIENT}>
                <RouterProvider router={router} />
            </QueryClientProvider>
            <Toaster position={"bottom-center"}/>
        </>
    )
}

export default App;

