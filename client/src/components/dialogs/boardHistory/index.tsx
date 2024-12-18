import { formatDate } from "@hooks/utils/useTime";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { MyBoards, UserBoard } from "@Api";



interface BoardHistoryDialog extends IBaseDialog {
    myBoard: MyBoards;
}


export const BoardHistoryDialog = (props: BoardHistoryDialog) => {

    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Board History" dialogSize={DialogSizeEnum.mediumFixed} >
                <div className="flex flex-col w-full h-full gap-16 p-5 overflow-y-auto">
                    <table className="w-full">
                        <thead>
                            <tr className="bg-base-300 h-12 hidden lg:table-row">
                                <th className="rounded-l-xl text-xs">Board Id</th>
                                <th className="text-xs text-start">Purchased</th>
                                <th className="text-xs text-start">Numbers</th>
                                <th className="rounded-r-xl text-xs text-start">Amount Won</th>
                            </tr>
                        </thead>

                        <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                            {props.myBoard && 
                                Object.values(props.myBoard.boards as UserBoard[]).map((value: UserBoard) => {
                                    return (
                                        <tr key={value.boardId} className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                            <td >{value.boardId}</td>
                                            <td >{formatDate(value.dateOfPurchase)}</td>
                                            <td >{value.numbers!.join(", ")}</td>
                                            <td >{value.winningAmount}</td>
                                        </tr>
                                    );
                                })
                            }
                        </tbody>
                    </table>
                </div>
            </BaseDialog>
        </>
    )
}