import axios from "axios";
import {FormEvent, useEffect, useState } from "react";
interface WinningNumbersResponseDTO {
    Gameid: string;
    Winningnumbers: number[];
    Status: string;
}
export const WinningNumbers = () => {
    const [gameId, setGameId] = useState<string>('');
    const [winningNumbers, setWinningNumbers] = useState<number[]>([]);
    const [response, setResponse] = useState<WinningNumbersResponseDTO | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    useEffect(() => {
        const fetchActiveGameId = async () => {
            try {
                const result = await axios.post("http://localhost:50001/api/game/NewGame");
                setGameId(result.data.id); // Set the active Game ID
            } catch (err) {
                setError('Failed to fetch active game.');
                console.error(err);
            }
        };
        fetchActiveGameId();
    },[]);
    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        if (winningNumbers.length !== 3) {
            setError('Winning numbers must be exactly 3.');
            return;
        }
        
        try {
            const result = await axios.post<WinningNumbersResponseDTO>(`http://localhost:5001/api/games/${gameId}/winning-numbers`,    
            {winningNumbers : winningNumbers}
        );
            setResponse(result.data);
            setError(null);
        } catch (err) {
            setError('There was an error setting the winning numbers.');
            console.error(err);
        }
    };
    const handleWinningNumbers = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value;
        const numbers = value.split(",").map(Number);
        setWinningNumbers(numbers);

        console.table(winningNumbers);
    };


    return (
        <div>
            <h2>Set Winning Numbers</h2>
            <form onSubmit={handleSubmit}>

                <div className="flex flex-col gap-5">
                    <div className="flex flex-col gap-2 w-full">
                        <div className="text-sm">Winning Numbers</div>
                        <input type="text" placeholder="123,123,123,123" onChange={handleWinningNumbers}
                               className="w-full bg-base-300 p-3 rounded-xl"/>
                    </div>
                </div>
                <button type="submit">Submit</button>
            </form>
            {response && (
                <div>
                    <h3>Response:</h3>
                    <p>Game ID: {response.Gameid}</p>
                    <p>Status: {response.Status}</p>
                    <p>Winning Numbers: {response.Winningnumbers.join(', ')}</p>
                </div>
            )}
        </div>
    );
};
export default WinningNumbers;