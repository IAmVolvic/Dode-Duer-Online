import { TransactionResponseDTO } from "@Api";
import { TransactionEditDialog } from "@components/dialogs/transactionEdit";
import { TransactionStatus, transactionStatusColor } from "@hooks/user/types/status";
import { useGetUsersTransactions } from "@hooks/user/usersTransactions";
import { useBoolean } from "@hooks/utils/useBoolean";
import { useState } from "react";
import { FiEdit } from "react-icons/fi";

export const TransactionControl = () => {
    const [selectedTransaction, setSelectedTransaction] = useState({} as TransactionResponseDTO);
    const [isOpen,, setTrue, setFalse] = useBoolean(false);
    const { isLoading, data, refetch } = useGetUsersTransactions();

    const handleOpenDialog = (transaction: TransactionResponseDTO) => {
        setSelectedTransaction(transaction);
        setTrue();
    }


    return (
        <>
            <div className="flex flex-col gap-5">
                <table className="w-full">
                    <thead>
                        <tr className="bg-base-300 h-12 hidden lg:table-row">
                            <th className="rounded-l-xl"></th>
                            <th className="text-xs text-start">Username</th>
                            <th className="text-xs text-start">Transaction Id</th>
                            <th className="rounded-r-xl text-xs text-start"></th>
                        </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                        {!isLoading && 
                            Object.values(data as TransactionResponseDTO[]).map((value: TransactionResponseDTO) => {
                                const { textContent, backgroundContent, background } = transactionStatusColor[TransactionStatus[value.transactionStatus as keyof typeof TransactionStatus]];
                        
                                return (
                                    <tr key={value.id} className="flex flex-col gap-2 pb-5 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                        <td className="text-sm py-3">
                                            <div className={`flex flex-row gap-2 items-center justify-center w-max px-3 py-1 rounded-md ${background}`}>
                                                <div className={`w-2.5 h-2.5 rounded-full ${backgroundContent}`} />
                                                <span className={textContent}>{value.transactionStatus}</span>
                                            </div>
                                        </td>
                        
                                        <td >{value.username}</td>
                                        <td >{value.transactionNumber}</td>
                                        <td >
                                            <button onClick={() => handleOpenDialog(value)} className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-12 h-7"> 
                                                <FiEdit /> 
                                            </button>
                                        </td>
                                    </tr>
                                );
                            })
                        }
                    </tbody>
                </table>
            </div>

            <TransactionEditDialog isOpen={isOpen} close={setFalse} refresh={refetch} transaction={selectedTransaction} />
        </>
    );
}

