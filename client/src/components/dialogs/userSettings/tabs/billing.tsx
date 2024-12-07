import { TransactionResponseDTO } from "@Api";
import { UserTransactionDialog } from "@components/dialogs/userTransaction";
import { useAuth } from "@hooks/authentication/useAuthentication";
import { TransactionStatus, transactionStatusColor } from "@hooks/user/types/status";
import { useGetTransactions } from "@hooks/user/useGetTransactions";
import { useBoolean } from "@hooks/utils/useBoolean";
import { useEffect } from "react";
import { FiCreditCard } from "react-icons/fi";
import { TbCurrencyKroneDanish } from "react-icons/tb";


export const BillingTabContent = () => {
    const {user, isLoggedIn} = useAuth();
    const { isLoading, data, refetch } = useGetTransactions();
    const [isOpen,, setTrue, setFalse] = useBoolean(false);


    return (
    <>
        <div className="flex flex-col gap-10">
            <div className="flex flex-row items-center gap-5">
                <div className="flex flex-row items-center gap-2 bg-base-300 rounded-xl px-3 h-11 w-full">
                    <TbCurrencyKroneDanish className="text-2xl text-primary" />
                    <div className="text-md"> {user?.balance} </div>
                </div>

                <button className="flex justify-center items-center bg-primary text-primary-content rounded-xl px-6 h-11" onClick={setTrue}> 
                    <FiCreditCard /> 
                </button>
            </div>            

            <table className="w-full">
                <thead>
                    <tr className="bg-base-300 h-12 hidden lg:table-row">
                        <th className="rounded-l-xl"></th>
                        <th className="rounded-r-xl text-xs text-start">Transaction Id</th>
                        {/* <th className="rounded-r-xl text-xs text-start">Transaction Number</th> */}
                    </tr>
                </thead>

                <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                    {!isLoading && 
                        Object.values(data as TransactionResponseDTO[]).map((value: TransactionResponseDTO) => {
                            const { textColor, textContent, backgroundContent, background } = transactionStatusColor[TransactionStatus[value.transactionStatus!]];
                    
                            return (
                                <tr key={value.id} className="flex flex-col gap-2 pb-5 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                    <td className="text-sm py-3">
                                        <div className={`flex flex-row gap-2 items-center justify-center w-max px-3 py-1 rounded-md ${background}`}>
                                            <div className={`w-2.5 h-2.5 rounded-full ${backgroundContent}/30`} />
                                            <span className={textContent}>{value.transactionStatus}</span>
                                        </div>
                                    </td>
                    
                                    <td >{value.id}</td>
                                   {/*  <td >{value.transactionNumber}</td> */}
                                </tr>
                            );
                        })
                    }
                </tbody>
            </table>
        </div>

        <UserTransactionDialog isOpen={isOpen} close={setFalse} transacrionRefresh={refetch} />
    </>
    );
}