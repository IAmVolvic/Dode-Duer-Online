import { TextInput, InputTypeEnum } from "@components/inputs/textInput";
import { useAuth } from "@hooks/authentication/useAuthentication";
import { useState } from "react";

export const AccountTabContent = () => {
    const {user, isLoggedIn} = useAuth();
    const [userName, setUserName] = useState<string>(user?.name ?? "");
    const [userPhone, setUserPhone] = useState<string>(user?.phoneNumber ?? "");
    const [userEmail, setUserEmail] = useState<string>(user?.email ?? "");
    const [loginPassword, setLoginPassword] = useState<string>("");



    return (
    <div className="flex flex-col gap-5">
        <div className="flex flex-row items-center gap-5">
            <img src={`https://api.dicebear.com/9.x/adventurer/svg?seed=${user?.name}`} alt="user" className="bg-base-100/50 w-10 h-10 lg:w-24 lg:h-24 rounded-full" />

            <div className="flex flex-col gap-1">
                <div className="text-2xl font-semibold">{user?.name}</div>
                <div className="text-sm font-light">Account Role - {user?.role}</div>
            </div>
        </div>
        
        <div className="flex flex-col gap-5">
            <div className="text-lg">Account Information</div>

            <div className="flex flex-col gap-2">
                <div className="flex flex-row gap-5">
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.text} inputTitle="Username" setInput={setUserName} input={userName} />
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.tel} inputTitle="Phone Number" setInput={setUserPhone} input={userPhone} />
                </div>

                <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.email} inputTitle="Email" setInput={setUserEmail} input={userEmail} />
                <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.password} inputTitle="Password" setInput={setLoginPassword} input={loginPassword} />
            </div>
        </div>
        
        <div className="flex justify-end mt-5">
            <button className="h-10 bg-primary text-primary-content rounded-2xl lg:w-32"> Save </button>
        </div>
    </div>
    );
}