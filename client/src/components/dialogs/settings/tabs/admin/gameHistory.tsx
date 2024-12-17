import { useEffect, useState } from "react";
import {Api, AuthorizedUserResponseDTO, GameResponseDTO, GameStatus} from "@Api.ts";
import { FiEdit } from "react-icons/fi";
import {BoardsShow} from "@components/dialogs/boardsShow/boardsShow.tsx";
import {useBoolean} from "@hooks/utils/useBoolean.tsx";

export const GameHistory = () => {
    const [data, setData] = useState<GameResponseDTO[]>([]); // State to store fetched data
    const [loading, setLoading] = useState<boolean>(true);  // State to manage loading
    const [selectedGame, setSelectedGame] = useState({} as string);
    const [isOpenBoardHistory,, setTrueBoardHistory, setFalseBoardHistory] = useBoolean(false);


    const fetchData = async () => {
        try {
            const api = new Api();
            const response = await api.game.gameGetAllGames();
            setData(response.data);
        } catch (error) {
            console.error("Error fetching game history:", error);
        } finally {
            setLoading(false);
        }
    };

    // Fetch data when the component mounts
    useEffect(() => {
        fetchData();
    }, []);

    // Placeholder or loading state
    if (loading) {
        return <div>Loading game history...</div>;
    }

    const addDaysToDateOnly = (dateOnly: string, days: number): string => {
        const date = new Date(dateOnly);
        date.setDate(date.getDate() + days);
        return date.toISOString().split("T")[0];
    };

    const handleOpenBoardHistoryDialog = (gameId: string) => {
        setSelectedGame(gameId);
        setTrueBoardHistory();
    }

    return (
        <div className="flex flex-col gap-5">
            <table className="w-full">
                <thead>
                <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                    <th className="rounded-l-xl text-xs text-start lg:text-center">Start date</th>
                    <th className="text-xs text-start lg:text-center">End date</th>
                    <th className="text-xs text-start lg:text-center">Status</th>
                    <th className="rounded-r-xl text-start lg:text-center"></th>
                </tr>
                </thead>

                <tbody className="before:content-['\200C'] before:leading-4 before:block">
                {data.map((value: GameResponseDTO) => (
                    <tr
                        key={value.id}
                        className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm lg:align-middle"
                    >
                        <td className="lg:text-center lg:align-middle">{value.date}</td>
                        <td className="lg:text-center lg:align-middle">
                            {addDaysToDateOnly(value.date!, 6)}
                        </td>
                        <td className="lg:text-center lg:align-middle">
                            {value.status}
                        </td>
                        <td className="lg:w-44">
                            <button
                                onClick={()=> handleOpenBoardHistoryDialog(value.id!)}
                                className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-40 h-7">
                                See boards
                            </button>
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
            <BoardsShow gameId={selectedGame} isOpen={isOpenBoardHistory} close={setFalseBoardHistory}/>
        </div>
    );
};
