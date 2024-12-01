import { useState } from "react";
import { InputTypeEnum, TextInput } from "@components/inputs/textInput";
import { Link } from "react-router-dom";
import ThemeSwitcher from "@components/themeSwitcher";
import { Toaster } from "react-hot-toast";
import { ProtectedComponent } from "@components/authProtected/ProtectedComponent";
import { useLogin } from "@hooks/authentication/useLogin";

// Importing Images
import BGImage_1 from "@assets/images/BG2.jpeg"
import Logo from "@assets/images/SiteLogo.png"


export const LoginPage = () => {
    const [loginEmail, setLoginEmail] = useState<string>("");
    const [loginPassword, setLoginPassword] = useState<string>("");
    const login = useLogin({email: loginEmail, password: loginPassword});

    return (
        <ProtectedComponent showWhileAuthenticated={false} redirect={"/"}>
            <Toaster position="top-center"/>
            
            <div className="h-screen">
                <ThemeSwitcher />
                
                <div className="w-full h-full flex items-center justify-center">
                    {/* Main Content Area */}
                    <div className="flex flex-col items-center w-full px-10 h-full lg:w-2/4">
                        <div className="flex flex-col items-center gap-16 w-full mt-24">
                            {/* Logo and Welcome Message */}
                            <div className="flex flex-col items-center justify-center gap-5">
                                <img className="w-32 h-32 object-contain" src={Logo} alt="Logo" loading="lazy" />
                                <div className="text-center">
                                    <div className="text-2xl lg:text-5xl font-semibold uppercase">Welcome Back!</div>
                                    <div className="text-2xl lg:text-xl">Please enter your details</div>
                                </div>
                            </div>
                            
                            {/* Form Inputs */}
                            <div className="flex flex-col gap-10 w-full lg:w-96">
                                <div className="flex flex-col gap-5">
                                    <TextInput inputType={InputTypeEnum.email} inputTitle="Email" setInput={setLoginEmail} input={loginEmail} />
                                    <TextInput inputType={InputTypeEnum.password} inputTitle="Password" setInput={setLoginPassword} input={loginPassword} />
                                </div>

                                {/* Login Button */}
                                <button className="w-full h-12 bg-primary text-primary-content rounded-3xl lg:w-96" onClick={login}> Login </button>
                            </div>
                            
                            {/* Return Home Link */}
                            <Link className="w-max text-sm text-center" to="/">Return Home</Link>
                        </div>
                    </div>

                    {/* Image Section for Larger Screens */}
                    <div className="hidden lg:block w-2/4 h-full">
                        <img className="w-full h-full rounded-s-3xl object-cover" src={BGImage_1} alt="Background" loading="lazy" />
                    </div>
                </div>
            </div>
        </ProtectedComponent>
    )
}