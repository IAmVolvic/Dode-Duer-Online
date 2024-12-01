// Importing Images
import Logo from "@assets/images/SiteLogo.png"
import { Link, NavLink } from "react-router-dom"
import { FiUser, FiLogIn } from "react-icons/fi";
import { useAuth } from "@hooks/authentication/useAuthentication";

interface NavigationProps {}

export const TopNavigation = (props: NavigationProps) => {
    const {isLoggedIn} = useAuth();
	return (
        <div className="flex flex-row justify-between backdrop-blur-xl backdrop-brightness-100 mb-10 border-b-0.05r border-base-content/50">

            <div className="flex flex-row items-center gap-24 px-5">
                <img className="w-20 h-20 object-contain" src={Logo} alt="Logo" loading="lazy" />

                <div className="flex flex-row items-center gap-10 h-full">
                    <NavLink className={(values) => `${values.isActive  ? 'activeNav' : ''} navButton text-lg font-medium h-full w-20` } to="/">
                        <div className="">Home</div>
                    </NavLink>

                    <NavLink className={(values) => `${values.isActive  ? 'activeNav' : ''} navButton text-lg font-medium h-full w-20` } to="/play">
                        <div className="">Play</div>
                    </NavLink>
                </div>
            </div>

            <div className="flex justify-center items-center w-32 border-l-0.05r border-base-content/50">
                {isLoggedIn ? (
                    <Link to="/profile" className="w-full h-full flex justify-center items-center">
                        <FiUser className="w-7 h-7"/>
                    </Link>
                ) : (
                    <Link to="/login" className="w-full h-full flex justify-center items-center">
                        <FiLogIn className="w-7 h-7"/>
                    </Link>
                )}
            </div>
        </div>
    )
}
