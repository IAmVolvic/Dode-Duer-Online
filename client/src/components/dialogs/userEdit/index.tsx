import { TextInput, InputTypeEnum } from "@components/inputs/textInput";
import { TbCurrencyKroneDanish } from "react-icons/tb";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, AuthorizedUserResponseDTO, UserEnrolled, UserRole, UserStatus, UserUpdateByAdminRequestDTO } from "@Api";
import { useEffect, useState } from "react";
import { SelectInput } from "@components/inputs/select";
import toast from "react-hot-toast";
import { ErrorToast } from "@components/errorToast";

interface UserEditDialogProps extends IBaseDialog {
    user: AuthorizedUserResponseDTO;
    refresh: () => void;
}

interface AccountUpdateRQForm {
    name: string;
    email: string;
    phoneNumber: string;

    enrolledStatus: UserEnrolled;
    userStatus: UserStatus;
    userRole: UserRole;

    password: string;
    passwordConfirm: string;
}

export const UserEditDialog = (props: UserEditDialogProps) => { 
    const API = new Api();

    const [data, setData] = useState<AccountUpdateRQForm>({
        name: props.user?.name || '',
        email: props.user?.email || '',
        phoneNumber: props.user?.phoneNumber || '',

        enrolledStatus: props.user?.enrolled  as UserEnrolled || UserEnrolled.True,
        userStatus: props.user?.status as UserStatus || UserStatus.Active,
        userRole: props.user?.role as UserRole || UserRole.User,

        password: '',
        passwordConfirm: '',
    });

    useEffect(() => {
        setData({
            name: props.user?.name || '',
            email: props.user?.email || '',
            phoneNumber: props.user?.phoneNumber || '',

            enrolledStatus: props.user?.enrolled  as UserEnrolled || UserEnrolled.True,
            userStatus: props.user?.status as UserStatus || UserStatus.Active,
            userRole: props.user?.role as UserRole || UserRole.User,

            password: '',
            passwordConfirm: '',
        });
    }, [props.user]);

    
    const handleInputChange = (field: keyof AccountUpdateRQForm) => (value: string) => {
        setData((prevData) => ({
            ...prevData,
            [field]: value,
        }));
    };


    const handleSelectChange = (field: keyof AccountUpdateRQForm) => (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
        const value = e.target.value;
        setData((prevData) => ({
            ...prevData,
            [field]: value,
        }));
    };
  
    const handleUpdatingUser = () => {
        let dataToSend: UserUpdateByAdminRequestDTO = {
            id: props.user.id?.toString()!
        };

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
        dataToSend.enrolledStatus = data.enrolledStatus;
        dataToSend.userStatus = data.userStatus;
        dataToSend.userRole = data.userRole;

        API.user.userPUpdateUserByAdmin(dataToSend).then(() => {
            props.refresh();
            props.close();
        }).catch((err) => {
            ErrorToast(err);
        });
    };

    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="User Edit" dialogSize={DialogSizeEnum.mediumFixed} >
                <div className="flex flex-col justify-between gap-7 w-full h-full p-5 overflow-y-auto">
                    <div className="flex flex-row items-center gap-5">
                        <img src={`https://api.dicebear.com/9.x/adventurer/svg?seed=${props.user.name}`} alt="user" className="bg-base-100/50 w-10 h-10 lg:w-24 lg:h-24 rounded-full" />

                        <div className="flex flex-col gap-1">
                            <div className="text-2xl font-semibold">{props.user.name}</div>
                            <div className="text-sm font-light">Account ID - {props.user.id}</div>
                        </div>
                    </div>


                    <div className="flex flex-col gap-5">
                        <div className="text-lg">Account Information</div>

                        <div className="flex flex-row items-center gap-2 bg-base-300 rounded-xl px-6 h-11 w-full">
                            <TbCurrencyKroneDanish className="text-2xl text-primary" />
                            <div className="text-md"> {props.user?.balance} </div>
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

                            <SelectInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputTitle="User Role" handleChange={handleSelectChange('userRole')} selectArray={[UserRole.Admin, UserRole.User]} defaultValue={data.userRole} />

                            <div className="flex flex-row items-cente gap-5">
                                <SelectInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputTitle="User Status" handleChange={handleSelectChange('userStatus')} selectArray={[UserStatus.Active, UserStatus.Inactive]} defaultValue={data.userStatus} />
                                <SelectInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputTitle="Enrolled Status" handleChange={handleSelectChange('enrolledStatus')} selectArray={[UserEnrolled.True, UserEnrolled.False]} defaultValue={data.enrolledStatus} />
                            </div>
                        </div>
                    </div>
                    
                    <div className="flex justify-end mt-5">
                        <button className="h-10 bg-primary text-primary-content rounded-2xl w-32" onClick={handleUpdatingUser}> Save </button>
                    </div>
                </div>
            </BaseDialog>
        </>
    )
}