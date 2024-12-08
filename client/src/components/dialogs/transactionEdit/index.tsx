import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, TransactionResponseDTO } from "@Api";

interface TransactionEditDialogProps extends IBaseDialog {
    transaction: TransactionResponseDTO;
    refresh: () => void;
}


export const TransactionEditDialog = (props: TransactionEditDialogProps) => { 
    const API = new Api();
    
    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Transaction Edit" dialogSize={DialogSizeEnum.small} >
                {props.transaction.id}
            </BaseDialog>
        </>
    )
}