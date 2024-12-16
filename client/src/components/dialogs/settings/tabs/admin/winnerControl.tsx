import { WinnerResponseDTO } from "@Api";
import { TransactionEditDialog } from "@components/dialogs/transactionEdit";
import { TransactionStatus, transactionStatusColor } from "@hooks/user/types/status";
import { useGetUsersTransactions } from "@hooks/user/usersTransactions";
import { useBoolean } from "@hooks/utils/useBoolean";
import { useState } from "react";
import { FiEdit } from "react-icons/fi";

export const WinnersControl = ({ gameId }: { gameId: string }) => {
    const [selectedWinner, setSelectedWinner] = useState({} as WinnerResponseDTO);
    const [isOpen, , setTrue, setFalse] = useBoolean(false);
    const { isLoading, data, refetch } = useGetWinners(gameId);

    const handleOpenDialog = (winner: WinnerResponseDTO) => {
        setSelectedWinner(winner);
        setTrue();
    };

    return (
        <>
            <div className="flex flex-col gap-5">
                <table className="w-full">
                    <thead>
                    <tr className="bg-base-300 h-12 hidden lg:table-row">
                        <th className="rounded-l-xl"></th>
                        <th className="text-xs text-start">Username</th>
                        <th className="text-xs text-start">Amount Won</th>
                        <th className="text-xs text-start">Week Number</th>
                        <th className="rounded-r-xl text-xs text-start"></th>
                    </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block">
                    {!isLoading &&
                        Object.values(data as WinnerResponseDTO[]).map((winner: WinnerResponseDTO) => (
                            <tr key={winner.id} className="flex flex-col gap-2 pb-5 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                <td className="text-sm py-3">
                                    <span>{winner.username}</span>
                                </td>
                                <td>{winner.wonAmount} zł</td>
                                <td>{winner.weekNumber}</td>
                                <td>
                                    <button
                                        onClick={() => handleOpenDialog(winner)}
                                        className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-12 h-7"
                                    >
                                        <FiEdit />
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            <WinnerEditDialog isOpen={isOpen} close={setFalse} refresh={refetch} winner={selectedWinner} />
        </>
    );
};