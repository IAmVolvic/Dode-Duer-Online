import { LargeContainer } from "@components/containers";
import { Button } from "@headlessui/react";
import { useState } from "react";
import { useAtom } from 'jotai';
import { PriceAtom } from "@atoms/PriceAtom";
import { Api, BoardResponseDTO, PlayBoardDTO } from "@Api";
import { useAuth } from "@hooks/authentication/useAuthentication";
import { format } from 'date-fns';
import { AxiosResponse } from "axios";
import toast from "react-hot-toast";

export const PlayPage = () => {
    const [pickedNumbers, setPickedNumbers] = useState<number[]>([]);
    const {user, isLoggedIn} = useAuth();
    const buttons = Array.from({ length: 16 }, (_, i) => i + 1);
    const [prices] = useAtom(PriceAtom);

    const totalPrice = () => {
        const count = pickedNumbers.length;

        // Return 0 if less than 5 numbers are picked
        if (count < 5) return 0;

        // Find the price object where numbers equals the count
        const priceObj = prices.find((p) => p.numbers === count);

        // Return price1 if found, otherwise 0
        return priceObj?.price1 || 0;
    };

    const toggleNumber = (n: number) => {
        setPickedNumbers((prev) => {
            if (prev.includes(n)) {
                // Remove the number if it's already picked
                return prev.filter((num) => num !== n);
            } else if (prev.length < 8) {
                // Add the number only if the picked count is less than 8
                return [...prev, n];
            }
            return prev; // Do nothing if already 8 numbers picked
        });
    };
    
    const playBoard =() => {
        if (user != null){
            let board : PlayBoardDTO = {
                userid : user.id!,
                numbers : pickedNumbers!,
                dateofpurchase : format(new Date(), 'yyyy-MM-dd')
            };
            console.log(board);
            let api = new Api();
            api.board.boardPlayBoard(board).then((r: AxiosResponse<BoardResponseDTO>) =>{
                toast.success("Your board has been played");
                console.log(r.data);
            });
        }
    }

    return (
        <LargeContainer className="flex flex-col gap-14 mb-52">
            <div className="flex justify-center">
                <h1 className="text-4xl sm:text-8xl font-bold">Play Board</h1>
            </div>
            <div className="flex flex-row flex-wrap gap-8 justify-center">
                {buttons.map((button) => (
                    <Button
                        key={button}
                        onClick={() => toggleNumber(button)}
                        className={`btn w-32 h-auto md:h-24 md:w-60 text-center text-lg ${
                            pickedNumbers.includes(button) ? "btn-primary" : "btn-neutral"
                        }`}
                        disabled={
                            !pickedNumbers.includes(button) && pickedNumbers.length >= 8
                        } // Disable if max numbers selected and this number isn't picked
                    >
                        {button}
                    </Button>
                ))}
            </div>
            <label className="flex justify-center lg:text-2xl text-xl">Price: {totalPrice()}</label>
            <div className="flex justify-center">
                <Button disabled={pickedNumbers.length <5} className="btn btn-primary w-48 text-center text-xl" onClick={() => playBoard()}>
                    Accept
                </Button>
            </div>
        </LargeContainer>
    );
};
