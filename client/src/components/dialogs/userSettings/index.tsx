import { Dialog, DialogPanel, DialogTitle, Button, DialogBackdrop, Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/react";
import { useLogout } from "@hooks/authentication/useLogout";
import { FiX, FiSettings, FiCreditCard, FiLayout, FiLogOut } from "react-icons/fi";
import classNames from 'classnames';

// TABS
import { AccountTabContent } from "./tabs/account";
import { BillingTabContent } from "./tabs/billing";
import { AppearanceTabContent } from "./tabs/appearance";


interface UserSettingsDialogProps {
    isOpen: boolean;
    close: () => void;
}


export const UserSettingsDialog = (props: UserSettingsDialogProps) => { 
    const logout = useLogout();

    const handleLogout = () => {
        props.close();
        logout();
    };

    return (
        <>
            <Dialog open={props.isOpen} as="div" className="relative z-50 focus:outline-none" onClose={props.close}>
                <DialogBackdrop className="fixed inset-0 bg-base-100/60" />
                <div className="flex items-center justify-center w-screen fixed inset-0">
                    <DialogPanel className="flex flex-col items-center w-192 h-160 border-0.05r border-base-content/50 backdrop-blur-xl backdrop-brightness-100 rounded-2xl">
                        {/* TopBar */}
                        <div className="flex flex-row justify-between items-center min-h-16 max-h-16 w-full border-b-0.05r border-base-content/50 px-5">
                            <div className="flex items-center w-40 h-full border-r-0.05r border-base-content/50">
                                <div className="font-bold">Settings</div>
                            </div>

                            <Button onClick={props.close} className="h-8 w-8 flex justify-center items-center">
                                <FiX />
                            </Button>
                        </div>

                        {/* Content */}
                        <TabGroup className="flex flex-row w-full h-full overflow-hidden px-5">
                            <TabList className="flex flex-col min-w-40 max-w-40 border-r-0.05r border-base-content/50">
                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiSettings className="opacity-60" size={20} /> </div>
                                    <div className=""> Account </div>
                                </Tab>

                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiCreditCard className="opacity-60" size={20} /> </div>
                                    <div className=""> Billing </div>
                                </Tab>

                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiLayout className="opacity-60" size={20} /> </div>
                                    <div className=""> Appearance </div>
                                </Tab>
                                
                                <Button onClick={handleLogout} className="flex flex-row items-center gap-5 py-2.5 outline-none absolute bottom-0 mb-3 text-error"> 
                                    <div className="flex justify-center items-center"> <FiLogOut className="opacity-60" size={20} /> </div>
                                    <div className=""> Logout </div> 
                                </Button>
                            </TabList>

                            <TabPanels className="p-5 w-full h-full overflow-y-scroll">
                                <TabPanel> <AccountTabContent /> </TabPanel>
                                <TabPanel> <BillingTabContent /> </TabPanel>
                                <TabPanel> <AppearanceTabContent /> </TabPanel>
                            </TabPanels>
                        </TabGroup>
                    </DialogPanel>
                </div>
            </Dialog>
        </>
    )
}