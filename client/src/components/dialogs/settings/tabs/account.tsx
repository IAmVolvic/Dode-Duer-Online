import { Api, UserUpdateRequestDTO } from "@Api";
import { TextInput, InputTypeEnum } from "@components/inputs/textInput";
import { useAuth } from "@hooks/authentication/useAuthentication";
import { useState } from "react";
import toast from "react-hot-toast";
import { TbCurrencyKroneDanish } from "react-icons/tb";


interface AccountUpdateRQForm {
    name: string;
    email: string;
    phoneNumber: string;
    password: string;
    passwordConfirm: string;
}


export const AccountTabContent = () => {
    const {user, refresh} = useAuth();
    const API = new Api();

    const [data, setData] = useState<AccountUpdateRQForm>({
        name: user?.name || '',
        email: user?.email || '',
        phoneNumber: user?.phoneNumber || '',
        password: '',
        passwordConfirm: '',
    });

    const handleInputChange = (field: keyof AccountUpdateRQForm) => (value: string) => {
        setData((prevData) => ({
            ...prevData,
            [field]: value,
        }));
    };


    const handleUpdatingUser = () => {
        let dataToSend: UserUpdateRequestDTO = {};

        if (data.password) {
            if (data.password !== data.passwordConfirm) {
                toast.error('Passwords do not match');
                return;
            }
      
            dataToSend.password = data.password;
        }

        dataToSend.name = data.name;
        dataToSend.email = data.email;
        dataToSend.phoneNumber = data.phoneNumber;

        API.user.userPUpdateUser(dataToSend).then(() => {
            toast.success('User updated successfully');
            refresh();
        }).catch(() => {
            toast.error('Failed to update user');
        });
    };


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

            <div className="flex flex-row items-center gap-2 bg-base-300 rounded-xl px-6 h-11 w-full">
                <TbCurrencyKroneDanish className="text-2xl text-primary" />
                <div className="text-md"> {user?.balance} </div>
            </div>
                
            <div className="flex flex-col gap-5">
                <div className="flex flex-row gap-5">
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.text} inputTitle="Username" setInput={handleInputChange('name')} input={data.name} />
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.tel} inputTitle="Phone Number" setInput={handleInputChange('phoneNumber')} input={data.phoneNumber} />
                </div>

                <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.email} inputTitle="Email" setInput={handleInputChange('email')} input={data.email} />

                <div className="flex flex-row gap-5">
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.password} inputTitle="Password" setInput={handleInputChange('password')} input={data.password} />
                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.password} inputTitle="Password Confirm" setInput={handleInputChange('passwordConfirm')} input={data.passwordConfirm} />
                </div>
                
            </div>
        </div>
        
        <div className="flex justify-end mt-5">
            <button className="h-10 bg-primary text-primary-content rounded-2xl w-32" onClick={handleUpdatingUser}> Save </button>
        </div>
    </div>
    );
}