import { GameResponseDTO, GameStatus } from "@Api";
import { GameEditDialog } from "@components/dialogs/gameEdit";
import { useGetCurrentGame } from "@hooks/game/useGetCurrentGame";
import { useBoolean } from "@hooks/utils/useBoolean";
import { useEffect, useState } from "react";
import { FiEdit } from "react-icons/fi";

export const GameControl = () => {
    const [currentGame, setCurrentGame] = useState<GameResponseDTO>();
    const [selectedGame, setSelectedGame] = useState<GameResponseDTO>();
    const [isOpen,, setTrue, setFalse] = useBoolean(false);
    const { data, isLoading, refetch } = useGetCurrentGame();

    useEffect(() => {
        if (!isLoading && data) {
            const activeGame = data.find((game) => game.status === GameStatus.Active);

            if (activeGame) {
                setCurrentGame(activeGame);
            }
        }
    }, [data, isLoading]);

    const handleGameEdit = (game: GameResponseDTO) => {
        setSelectedGame(game);
        setTrue();
    }

    return (isLoading) ? (
        <div>Loading...</div>
    ) : (
        <>
            <div className="flex flex-col gap-8">
                <div className="flex flex-col gap-5">
                    <div className="flex flex-row justify-between items-center">
                        <div className="text-2xl text-base-content font-medium"> Game Status: </div>
                        <div className="flex flex-row items-center gap-5">
                            <div className={`w-5 h-5 rounded-full status-ring ${currentGame ? 'status-ring-online' : 'status-ring-offline'}`}></div>
                            <div className="text-2xl text-base-content font-medium">{currentGame ? 'Running' : 'Ended'}</div>
                        </div>
                    </div>

                    GameId: {currentGame && currentGame.id}
                </div>


                {
                    !currentGame && (
                        <button onClick={() => {}} className="flex flex-row justify-center items-center gap-5 bg-primary text-primary-content rounded-xl w-full h-10"> 
                            <div>Start Game</div>
                        </button>
                    )
                }

                <table className="w-full">
                    <thead>
                        <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                            <th className="text-xs rounded-l-xl">Id</th>
                            <th className="text-xs text-start">Date</th>
                            <th className="text-xs text-start">Status</th>
                            <th className="rounded-r-xl"></th>
                        </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block">

                        {!isLoading && data && 
                            Object.values(data as GameResponseDTO[]).map((value: GameResponseDTO) => {
                                return (
                                    <tr key={value.id} className="flex flex-col gap-2 pb-5 lg:table-row border-b-0.05r border-base-content/50 text-sm h-12">
                                        <td >{value.id}</td>
                                        <td >{value.date}</td>
                                        <td >{value.status}</td>
                                        <td >
                                            <button onClick={() => handleGameEdit(value)} className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-12 h-6"> 
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

            <GameEditDialog isOpen={isOpen} close={setFalse} game={selectedGame!} refresh={refetch} />
        </>
    );
}