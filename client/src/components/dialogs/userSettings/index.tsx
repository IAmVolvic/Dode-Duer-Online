import { Dialog, DialogPanel, DialogTitle, Button, DialogBackdrop } from "@headlessui/react";
import { useLogout } from "@hooks/authentication/useLogout";


interface UserSettingsDialogProps {
    isOpen: boolean;
    close: () => void;
}


export const UserSettingsDialog = (props: UserSettingsDialogProps) => { 
    const logout = useLogout();

    return (
        <>
            <Dialog open={props.isOpen} as="div" className="relative z-10 focus:outline-none" onClose={props.close}>
                <DialogBackdrop className="fixed inset-0 bg-black/30" />
                <div className="fixed inset-0 z-10 w-screen overflow-y-auto">
                    <div className="flex min-h-full items-center justify-center p-4">
                        <DialogPanel transition className="w-full max-w-md rounded-xl bg-white/5 p-6 backdrop-blur-2xl duration-300 ease-out data-[closed]:transform-[scale(95%)] data-[closed]:opacity-0">
                            <DialogTitle as="h3" className="text-base/7 font-medium text-white"> User Settings </DialogTitle>

                            <p className="mt-2 text-sm/6 text-white/50">
                                Will come Later
                            </p>
                            
                            <div className="mt-4">
                                <Button className="inline-flex items-center gap-2 rounded-md bg-gray-700 py-1.5 px-3 text-sm/6 font-semibold text-white shadow-inner shadow-white/10 focus:outline-none data-[hover]:bg-gray-600 data-[focus]:outline-1 data-[focus]:outline-white data-[open]:bg-gray-700" onClick={() => { props.close(); logout(); }} >
                                    Logout
                                </Button>
                            </div>
                        </DialogPanel>
                    </div>
                </div>
            </Dialog>
        </>
    )
}