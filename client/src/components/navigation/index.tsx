// Importing Images
import Logo from "@assets/images/SiteLogo.png"
import { NavLink } from "react-router-dom"

interface NavigationProps {}

export const TopNavigation = (props: NavigationProps) => {
	return (
        <div className="flex flex-row backdrop-blur-xl backdrop-brightness-100 mb-10 navbarVolvic">

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


        </div>
    )
}