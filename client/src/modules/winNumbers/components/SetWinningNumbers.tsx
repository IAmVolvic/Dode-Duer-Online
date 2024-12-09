import axios from "axios";
import {FormEvent, useState } from "react";

interface WinningNumbersResponseDTO {
    Gameid: string;
    Winningnumbers: number[];
    Status: string;
}
export const WinningNumbers = () => {
    const [gameId, setGameId] = useState<string>('');
    const [winningNumbers, setWinningNumbers] = useState<number[]>([]);
    const [response, setResponse] = useState<WinningNumbersResponseDTO | null>(null);
    const [, setError] = useState<string | null>(null);
    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault();

        if (winningNumbers.length !== 3) {
            setError('Winning numbers must be exactly 3.');
            return;
        }

        try {
            const result = await axios.post<WinningNumbersResponseDTO>(`http://localhost:5000/api/games/${gameId}/winning-numbers`, {
                winningNumbers: winningNumbers
            });

            setResponse(result.data);
            setError(null); // Wyczyść błędy, jeśli wszystko poszło dobrze
        } catch (err) {
            setError('There was an error setting the winning numbers.');
            console.error(err);
        }
    };


    return (
        <div>
            <h2>Set Winning Numbers</h2>
            <form onSubmit={handleSubmit}>
                <div>
                    <label htmlFor="gameId">Game ID:</label>
                    <input
                        type="text"
                        id="gameId"
                        value={gameId}
                        onChange={(e) => setGameId(e.target.value)}
                        placeholder="Enter Game ID"
                        required
                    />
                </div>

                <div>
                    <label htmlFor="winningNumbers">Winning Numbers (separate with commas):</label>
                    <input
                        type="text"
                        id="winningNumbers"
                        value={winningNumbers.join(',')}
                        onChange={(e) => setWinningNumbers(e.target.value.split(',').map(num => parseInt(num.trim())))}
                        placeholder="Enter numbers (e.g. 1, 2, 3)"
                        required
                    />
                </div>

                <button type="submit">Submit</button>
            </form>

            {<p style={{ color: 'red' }}>{}</p>}

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
