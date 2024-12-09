import { LargeContainer } from "@components/containers";
import {InputTypeEnum, TextInput } from "@components/inputs/textInput";
import { Button } from "@headlessui/react";
import axios from "axios";
import {FormEvent, useEffect, useState } from "react";
interface WinningNumbersResponseDTO {
    Gameid: string;
    Winningnumbers: number[];
    Status: string;
}
export const WinNumbersTabContent = () => {
    const [gameId, setGameId] = useState<string>('');
    const [winningNumber, setWinningNumber] = useState<number| null>(null);
    const [response, setResponse] = useState<WinningNumbersResponseDTO | null>(null);
    const [error, setError] = useState<string | null>(null);

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

        if (winningNumber === null) {
            setError('Winning numbers must be exactly 3.');
            return;
        }
        
        try {
            const result = await axios.post<WinningNumbersResponseDTO>(`http://localhost:5001/api/games/${gameId}/winning-numbers`,    
            { winningNumber }
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
        const number = Number(value);
        if (!isNaN(number)) {
            setWinningNumber(number);
        }
    };


    return (
        <LargeContainer className="flex flex-col gap-14 mb-52">
            <div className="flex justify-center">
                <h1 className="text-4xl sm:text-8xl font-bold">Set Winning Numbers</h1>
            </div>

            <div className="flex flex-col gap-5">
                <label className="text-lg font-medium">Enter Winning Numbers (comma-separated):</label>
                
                <input
                    type="text"
                    placeholder="e.g., 1,2,3"
                    onChange={handleWinningNumbers}
                    className="w-full bg-base-300 p-3 rounded-xl text-lg"
                />
            </div>

    <div className="flex justify-center">
                <Button
                    onClick={handleSubmit}
                    className="btn btn-primary w-48 text-center text-xl"
                >
                    Submit
                </Button>
            </div>
            {response && (
                <div>
                    <p>Game ID: {response.Gameid}</p>
                    <p>Status: {response.Status}</p>
                    <p>Winning Numbers: {response.Winningnumbers.join(', ')}</p>
                </div>
            )}
        </LargeContainer>
    );
}