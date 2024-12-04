import { ReactNode } from "react";
import { Dialog, DialogPanel, Button, DialogBackdrop } from "@headlessui/react";
import { FiX } from "react-icons/fi";


interface DialogProps extends IBaseDialog {
    dialogTitle: string;
    dialogSize: DialogSizeEnum;
    children?: ReactNode;
}

export enum DialogSizeEnum {
    small = "min-w-128 min-h-128",
    medium = "min-w-192 min-h-160",
    large = "w-full h-full",

    smallFixed = "w-128 h-128",
    mediumFixed = "w-192 h-160"
}


export interface IBaseDialog {
    isOpen: boolean;
    close: () => void;
}


export const BaseDialog = (props: DialogProps) => { 
    return (
        <>
            <Dialog open={props.isOpen} as="div" className="relative focus:outline-none z-[60]" onClose={props.close}>
                <DialogBackdrop className="fixed inset-0 backdrop-blur-sm backdrop-brightness-50" />
                <div className="flex items-center justify-center w-screen fixed inset-0">
                    <DialogPanel className={`flex flex-col items-center border-0.05r border-base-content/50 backdrop-blur-xl backdrop-brightness-100 rounded-2xl ${props.dialogSize}`}>
                        {/* TopBar */}
                        <div className="flex justify-between items-center min-h-16 max-h-16 w-full border-b-0.05r border-base-content/50 px-5">
                            <div className="font-bold">{props.dialogTitle}</div>

                            <Button onClick={props.close} className="h-8 w-8 flex justify-center items-center">
                                <FiX />
                            </Button>
                        </div>


                        {/* Content */}
                        {props.children}
                    </DialogPanel>
                </div>
            </Dialog>
        </>
    )
}