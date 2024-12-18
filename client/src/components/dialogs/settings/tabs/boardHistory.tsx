import { MyBoards } from "@Api";
import { BoardHistoryDialog } from "@components/dialogs/boardHistory";
import { useGetMyBoardHistory } from "@hooks/game/useGetMyBoardHistory";
import { useBoolean } from "@hooks/utils/useBoolean";
import { formatDate } from "@hooks/utils/useTime";
import { useState } from "react";

export const BoardHistory = () => {
    const [selectedBoard, setSelectedBoard] = useState<MyBoards>();
    const { data, isLoading, refetch } = useGetMyBoardHistory();
    const [isOpen,, setTrue, setFalse] = useBoolean(false);

    const handleOpenHistory = (board: MyBoards) => {
        setSelectedBoard(board);
        setTrue();
    }

    return <>
        <div className="flex flex-col gap-5">
            <table className="w-full">
                <thead>
                <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                    <th className="rounded-l-xl text-xs">Started</th>
                    <th className="text-xs text-start">Ended</th>
                    <th className="text-xs text-start">Status</th>
                    <th className="rounded-r-xl"></th>
                </tr>
                </thead>

                <tbody className="before:content-['\200C'] before:leading-4 before:block">
                    {!isLoading && data &&
                    Object.values(data as MyBoards[]).map((value: MyBoards) => {
                        return (
                            <tr key={value.gameId} className="flex flex-col gap-2 pb-5 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                <td >{formatDate(value.startDate)}</td>
                                <td >{formatDate(value.endDate)}</td>
                                <td >{value.status}</td>
                                <td >
                                    <button onClick={() => handleOpenHistory(value)} className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-full h-7"> 
                                        View More
                                    </button>
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
        </div>

        <BoardHistoryDialog isOpen={isOpen} close={setFalse} myBoard={selectedBoard!} />
    </>
}