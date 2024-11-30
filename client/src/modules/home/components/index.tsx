import { useAuth } from "@hooks/authentication/useAuthentication";
import { useLogout } from "@hooks/authentication/useLogout";
import { Link } from "react-router-dom";

export const Home = () => {
    const { user } = useAuth();
    const logout = useLogout();

    return (
        <div>
            <div className="text-9xl text-teal-600">Home: {user?.name ?? ""}</div>
            <Link className="w-max text-sm text-center mr-10" to="/login">Login</Link>
            
            <button onClick={logout}>Logout</button>
        </div>
    )
}