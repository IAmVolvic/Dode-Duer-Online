import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Button, Tab, TabGroup, TabList, TabPanel, TabPanels } from "@headlessui/react";
import { useLogout } from "@hooks/authentication/useLogout";
import { FiSettings, FiCreditCard, FiLayout, FiLogOut, FiUsers, FiSliders, FiShoppingCart, FiMenu } from "react-icons/fi";
import classNames from 'classnames';
import { useAuth } from "@hooks/authentication/useAuthentication";
import { useBoolean } from "@hooks/utils/useBoolean";


// TABS
import { AccountTabContent } from "./tabs/account";
import { BillingTabContent } from "./tabs/billing";
import { AppearanceTabContent } from "./tabs/appearance";
import { WinNumbersTabContent} from "./tabs/winNumbers";

export const UserSettingsDialog = (props: IBaseDialog) => { 
    const [isOpen, toggle] = useBoolean(true)
    const {user, isLoggedIn} = useAuth();
    const logout = useLogout();

    const handleLogout = () => {
        props.close();
        logout();
    };

    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Settings" dialogSize={DialogSizeEnum.mediumFixed} childrenTitle={ <button onClick={toggle}><FiMenu className="" /></button>} >
                <TabGroup className="flex flex-row w-full h-full overflow-hidden lg:px-5">
                    <TabList className={`${isOpen ? "flex" : "hidden"} flex-col border-r-0.05r border-base-content/50 absolute w-52 h-full bg-base-200 pl-5 lg:pl-0 lg:relative lg:bg-transparent lg:min-w-40 lg:max-w-40 z-[100]`}>
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

                        {isLoggedIn && user?.role === 'Admin' && (
                            <>
                                <div className="border-t-0.05r border-base-content/50 my-3"></div>

                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiUsers className="opacity-60" size={20} /> </div>
                                    <div className=""> Users </div>
                                </Tab>

                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiSliders className="opacity-60" size={20} /> </div>
                                    <div className=""> Win Numbers </div>
                                </Tab>

                                <Tab className={({selected}) => classNames("flex flex-row items-center gap-5 py-2.5 outline-none", selected ? 'border-r-0.25r !border-primary':'')}>
                                    <div className="flex justify-center items-center"> <FiShoppingCart className="opacity-60" size={20} /> </div>
                                    <div className=""> Transactions </div>
                                </Tab>
                            </>
                        )}
                        
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
            </BaseDialog>
        </>
    )
}