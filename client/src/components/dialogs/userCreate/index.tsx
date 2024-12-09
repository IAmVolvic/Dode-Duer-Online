import { useState } from "react";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, AuthorizedUserResponseDTO } from "@Api";
import { TextInput, InputTypeEnum } from "@components/inputs/textInput";
import { ErrorToast } from "@components/errorToast";

interface UserCreateDialogProps extends IBaseDialog {
    refresh: () => void;
}


export const UserCreateDialog = (props: UserCreateDialogProps) => { 
    const API = new Api();
    const [username, setUsername] = useState<string>("");
    const [email, setEmail] = useState<string>(""); 
    const [phoneNumber, setPhoneNumber] = useState<string>("");

    const handleCreateUser = () => {
        API.user.userPSignup({
            name: username,
            email: email,
            phoneNumber: phoneNumber
        }).then((res) => {
            props.refresh();
            props.close();

            setUsername("");
            setEmail("");
            setPhoneNumber("");
        }).catch((err) => {
            ErrorToast(err);
        });
    };
    
    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="User Create" dialogSize={DialogSizeEnum.smallFixed} >
                <div className="flex flex-col justify-between gap-10 w-full h-full p-5 mt-5">
                    <div className="flex flex-col gap-5">
                        <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.text} inputTitle="Username" setInput={setUsername} input={username} />
                        <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.email} inputTitle="Email" setInput={setEmail} input={email} />
                        <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.tel} inputTitle="Phone Number" setInput={setPhoneNumber} input={phoneNumber} />
                    </div>

                    <button className="h-10 bg-primary text-primary-content rounded-2xl w-full" onClick={handleCreateUser}> Create User </button>
                </div>
            </BaseDialog>
        </>
    )
}