import { WinningNumbersResponseDTO } from "@Api";
import { CustomSelect, Option } from "@components/inputs/multiSelect";
import { Button } from "@headlessui/react";
import axios from "axios";
import { useEffect, useState } from "react";

export const WinningNumberControl = () => {
    const [gameId, setGameId] = useState<string>('');
    const [winningNumbers, setWinningNumbers] = useState<number[]>([]);
    const [response, setResponse] = useState<WinningNumbersResponseDTO | null>(null);
    const [error, setError] = useState<string | null>(null);
    
    useEffect(() => {
        const fetchActiveGame = async () => {
            try {
                const result = await axios.get("http://localhost:5001/api/Game/Active");

                if (result.data && result.data.id) {
                    setGameId(result.data.id);
                }
                console.log("Active Game ID: ", result.data.id);
                } catch (err) {
                setError('Failed to fetch active game.');
                console.error(err);
            }
        };
        fetchActiveGame();
    }, []);

    const handleSubmit = async () => {
        if (winningNumbers.length !== 3) {
            setError('Winning numbers must be exactly 3.');
            return;
        }

        if (!gameId) {
            setError('No active game found. Cannot submit winning numbers.');
            return;
        }

        try {
            const result = await axios.post(
                `http://localhost:5001/api/games/${gameId}/winning-numbers`,
                { winningNumbers }
            );
            setResponse(result.data);
            setError(null);
        } catch (err) {
            setError('There was an error setting the winning numbers.');
            console.error(err);
        }
    };

    const handleSelectionChange = (selectedItems: Option[]) => {
        setWinningNumbers(selectedItems.map(item => parseInt(item.value, 16)));
    };
    
    const options: Option[] = Array.from({ length: 16 }, (_, i) => ({
        value: `${i + 1}`,
        label: `${i + 1}`
    }));
    
    return (
        <div className="flex flex-col gap-14 mb-52">
            
            {gameId ? (
                <>
                    <CustomSelect
                        options={options}
                        placeHolder="Select winning numbers"
                        onSelectionChange={handleSelectionChange}
                    />

                    <div className="flex justify-center gap-4">
                        <Button
                            onClick={handleSubmit}
                            className="btn btn-primary w-48 text-center text-xl"
                        >
                            Submit
                        </Button>
                    </div>
                </>
            ) : (
                <p>Loading active game...</p>
            )}

            {error && <p className="text-red-500">{error}</p>}
            {response && (
                <div>
                    <p>Game ID: {response.gameid}</p>
                    <p>Status: {response.status}</p>
                    {response?.winningnumbers?.length ? ( //checking if this exist before executing the join
                        <p>Winning Numbers: {response.winningnumbers.join(', ')}</p>
                    ) : (
                        <p>No winning numbers set yet.</p>
                    )}
                </div>
            )}
        </div>
    );
};