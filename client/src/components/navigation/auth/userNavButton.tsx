import { UserEnrollmentDialog } from "@components/dialogs/userEnrollment"
import { UserSettingsDialog } from "@components/dialogs/settings"
import { useAuth } from "@hooks/authentication/useAuthentication"
import { useBoolean } from "@hooks/utils/useBoolean"
import { FiLogIn, FiUser } from "react-icons/fi"
import { Link } from "react-router-dom"


export const UserNavButton = () => { 
    const {user, isLoggedIn} = useAuth()
    const isEnrolled = (user?.enrolled === "False") ? false : true;

    const [isOpen,, setTrue, setFalse] = useBoolean(false);
    const [isOpenE,, setTrueE, setFalseE] = useBoolean(!isEnrolled);

    return(<>
        {isLoggedIn ? (
            <button onClick={setTrue} className="w-full h-full flex justify-center items-center">
                <FiUser className="w-7 h-7"/>
            </button>
        ) : (
            <Link to="/login" className="w-full h-full flex justify-center items-center">
                <FiLogIn className="w-7 h-7"/>
            </Link>
        )}

        <UserSettingsDialog isOpen={isOpen} close={setFalse} />

        {isLoggedIn && isEnrolled === false && <UserEnrollmentDialog isOpen={isOpenE} close={setFalseE}  />}
    </>)
}