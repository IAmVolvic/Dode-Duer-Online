import { InputTypeEnum, TextInput } from "@components/inputs/textInput";
import QRCode from "react-qr-code";
import { useState } from "react";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, UserEnrollmentRequestDTO } from "@Api";
import { ErrorToast } from "@components/errorToast";
import toast from "react-hot-toast";

interface UserEnrollmentDialogProps extends IBaseDialog {}

interface AccountEnrollmentRQForm {
    password: string;
    passwordConfirm: string;
}

export const UserEnrollmentDialog = (props: UserEnrollmentDialogProps) => { 
    const [data, setData] = useState<AccountEnrollmentRQForm>({
        password: '',
        passwordConfirm: '',
    });
    const API = new Api();


    const handleInputChange = (field: keyof AccountEnrollmentRQForm) => (value: string) => {
        setData((prevData) => ({
            ...prevData,
            [field]: value,
        }));
    };

    const handleEnrollment = () => {
        if (!data.password) {
            toast.error('Password not set');
        }

        if (data.password !== data.passwordConfirm) {
            toast.error('Passwords do not match');
            return;
        }

        API.user.userPEnroll({password: data.password}).then((res) => {
            toast.success("Enrollment Successful");
            props.close();
        }).catch((err) => {
            ErrorToast(err);
        });
    };

    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={()=>{}} dialogTitle="New Transaction" dialogSize={DialogSizeEnum.small} >
                <div className="flex flex-col justify-between gap-10 w-full h-full p-5">
                    <div className="flex flex-col justify-center items-center gap-10 w-full">
                        <div className="flex flex-col items-center justify-center">
                            <div className="text-lg font-bold">Welcome to Døde Duer Online</div>
                            <div className="text-xs text-base-content">To complete your enrollment you need to set a password.</div>
                        </div>

                        <div className="flex flex-col gap-5 w-full">
                            <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.password} inputTitle="Password" setInput={handleInputChange('password')} input={data.password} />
                            <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.password} inputTitle="Password Confirm" setInput={handleInputChange('passwordConfirm')} input={data.passwordConfirm} />
                        </div>
                    </div>

                    <button className="h-10 bg-primary text-primary-content rounded-xl w-full" onClick={handleEnrollment}> Complete </button>
                </div>
            </BaseDialog>
        </>
    )
}