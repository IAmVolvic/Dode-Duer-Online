import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, AuthorizedUserResponseDTO } from "@Api";

interface UserEditDialogProps extends IBaseDialog {
    user: AuthorizedUserResponseDTO;
    refresh: () => void;
}


export const UserEditDialog = (props: UserEditDialogProps) => { 
    const API = new Api();
    
    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="User Edit" dialogSize={DialogSizeEnum.small} >
                {props.user.id}
            </BaseDialog>
        </>
    )
}