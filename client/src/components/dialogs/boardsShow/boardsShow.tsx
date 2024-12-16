import {BaseDialog, DialogSizeEnum, IBaseDialog} from "@components/dialogs";
import {Api, BoardGameResponseDTO} from "@Api.ts";
import {useEffect, useState} from "react";

interface BoardsGetDialogProps extends IBaseDialog {
    gameId : string;
}


export const BoardsShow = (props: BoardsGetDialogProps) => {

    const [data, setData] = useState<BoardGameResponseDTO[]>([]);

    const fetchData = async () => {
        try {
            const api = new Api();
            const response = await api.board.boardGetBoardsFromGame(props.gameId);
            setData(response.data);
        } catch (error) {
            console.error("Error fetching game history:", error);
        }
    };

    useEffect(() => {
        if (props.isOpen) {
            fetchData();
        }
    }, [props.isOpen]);

    return <>
        <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Board history" dialogSize={DialogSizeEnum.smallFixed} >
        <div className="flex flex-col gap-5 w-full h-full max-h-[70vh] overflow-auto">
            <table className="w-full">
                <thead>
                <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                    <th className="rounded-l-xl text-xs text-start lg:text-center">Player name</th>
                    <th className="text-xs text-start lg:text-center">Date</th>
                    <th className="text-xs rounded-r-xl text-start lg:text-center">Numbers</th>
                </tr>
                </thead>

                <tbody className="before:content-['\200C'] before:leading-4 before:block">
                {data.map((value: BoardGameResponseDTO) => (
                    <tr
                        key={value.id}
                        className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm lg:align-middle"
                    >
                        <td className="lg:text-center lg:align-middle">{value.user}</td>
                        <td className="lg:text-center lg:align-middle">
                            {value.dateofpurchase}
                        </td>
                        <td className="lg:text-center lg:align-middle">
                            {value.numbers!.join(", ")}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
        </BaseDialog>
    </>
}