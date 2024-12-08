import { useState } from "react";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, TransactionResponseDTO, TransactionAdjustment, TransactionStatusA } from "@Api";
import { InputTypeEnum, TextInput } from "@components/inputs/textInput";
import { SelectInput } from "@components/inputs/select";
import toast from "react-hot-toast";

interface TransactionEditDialogProps extends IBaseDialog {
    transaction: TransactionResponseDTO;
    refresh: () => void;
}


export const TransactionEditDialog = (props: TransactionEditDialogProps) => { 
    const API = new Api();
    const [adjustmentAmount, setAdjustmentAmount] = useState<string>("0");
    const [adjustment, setAdjustment] = useState<TransactionAdjustment>(TransactionAdjustment.Deposit);
    const [status, setStatus] = useState<TransactionStatusA>(props.transaction.transactionStatus as TransactionStatusA || TransactionStatusA.Pending);

    const handleAdjustmentChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setAdjustment(e.target.value as TransactionAdjustment);
    }; 

    const handleStatusChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        setStatus(e.target.value as TransactionStatusA);
    }; 

    const handleAdjustmentSubmit = () => {
        if (!adjustment || !status) {
            toast.error('Failed: Please select adjustment and status');
            return;
        }

        API.transaction.transactionPUseBalance({
            transactionId: props.transaction.id?.toString()!,
            amount: parseFloat(adjustmentAmount),
            adjustment: adjustment,
            transactionStatusA: status
        }).then((res) => {
            toast.success('Successfully updated transaction');
            props.refresh();
        }).catch((err) => {
            toast.error('Failed to update transaction');
        });
    };

    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Transaction Edit" dialogSize={DialogSizeEnum.small} >
                <div className="flex flex-col justify-between gap-7 w-full h-full p-5">
                    <div className="flex flex-row items-center gap-5">
                        <img src={`https://api.dicebear.com/9.x/adventurer/svg?seed=${props.transaction.username}`} alt="user" className="bg-base-100/50 w-10 h-10 lg:w-24 lg:h-24 rounded-full" />

                        <div className="flex flex-col gap-1">
                            <div className="text-2xl font-semibold">{props.transaction.username}</div>
                            <div className="text-sm font-light">Account ID - {props.transaction.userId}</div>
                        </div>
                    </div>

                    <TextInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputType={InputTypeEnum.number} inputTitle="Adjustment Amount" setInput={setAdjustmentAmount} input={adjustmentAmount} />

                    <div className="flex flex-row items-center gap-5 w-full">
                        <SelectInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputTitle="Adjustment" handleChange={handleAdjustmentChange} selectArray={[TransactionAdjustment.Deposit, TransactionAdjustment.Deduct]} defaultValueText="Select Adjustment" />
                        <SelectInput parentClassName="flex flex-col gap-2 w-full" titleClassName="text-sm" inputTitle="Status" handleChange={handleStatusChange} selectArray={[TransactionStatusA.Pending, TransactionStatusA.Approved, TransactionStatusA.Rejected]} defaultValue={status} />
                    </div>
                    
                    <button className="h-10 bg-primary text-primary-content rounded-2xl w-full" onClick={handleAdjustmentSubmit}> Submit Adjustment </button>
                </div>
            </BaseDialog>
        </>
    )
}