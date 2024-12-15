// Importing Images
import Logo from "@assets/images/SiteLogo.png"
import { NavLink } from "react-router-dom"
import { UserNavButton } from "./auth/userNavButton";

export const TopNavigation = () => {
	return (
        <div className="flex flex-row justify-between backdrop-blur-xl backdrop-brightness-100 h-20  mb-10 border-b-0.05r border-base-content/50">

            <div className="flex flex-row items-center gap-24 px-5">
                <img className="w-20 h-20 object-contain hidden lg:block" src={Logo} alt="Logo" loading="lazy" />

                <div className="flex flex-row items-center gap-10 h-full">
                    <NavLink className={(values) => `${values.isActive  ? 'border-b-0.25r !border-primary' : ''} navButton text-lg font-medium h-full w-20` } to="/">
                        <div className="">Home</div>
                    </NavLink>

                    <NavLink className={(values) => `${values.isActive  ? 'border-b-0.25r !border-primary' : ''} navButton text-lg font-medium h-full w-20` } to="/play">
                        <div className="">Play</div>
                    </NavLink>
                </div>
            </div>

            <div className="flex justify-center items-center w-32 border-l-0.05r border-base-content/50">
                <UserNavButton />
            </div>
        </div>
    )
}
