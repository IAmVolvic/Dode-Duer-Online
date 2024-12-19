import { UserEnrollmentDialog } from "@components/dialogs/userEnrollment"
import { UserSettingsDialog } from "@components/dialogs/settings"
import { useAuth } from "@hooks/authentication/useAuthentication"
import { useBoolean } from "@hooks/utils/useBoolean"
import { FiLogIn, FiUser } from "react-icons/fi"
import { Link } from "react-router-dom"
import { useEffect } from "react"


export const UserNavButton = () => { 
    const {user, isLoggedIn} = useAuth()
    const isEnrolled = (user?.enrolled === "False") ? false : true;

    const [isOpen1, toggle1, setTrue1, setFalse1] = useBoolean(false);
    const [isOpen2, toggle2, setTrue2, setFalse2] = useBoolean(true);

    useEffect(() => {
        if (!isEnrolled) {
            setTrue2();
        }

        console.log(isEnrolled)
    }, [isEnrolled])

    return(<>
        {isLoggedIn ? (
            <button onClick={setTrue1} className="w-full h-full flex justify-center items-center">
                <FiUser className="w-7 h-7"/>
            </button>
        ) : (
            <Link to="/login" className="w-full h-full flex justify-center items-center">
                <FiLogIn className="w-7 h-7"/>
            </Link>
        )}

        <UserSettingsDialog isOpen={isOpen1} close={setFalse1} />

        {isLoggedIn && isEnrolled === false && <UserEnrollmentDialog isOpen={isOpen2} close={setFalse2}  />}
    </>)
}