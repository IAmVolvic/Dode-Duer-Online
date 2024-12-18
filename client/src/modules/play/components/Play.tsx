import { LargeContainer } from "@components/containers";
import { Button } from "@headlessui/react";
import { useState } from "react";
import { useAtom } from 'jotai';
import { PriceAtom } from "@atoms/PriceAtom";
import {Api, BoardResponseDTO, PlayAutoplayBoardDTO, PlayBoardDTO} from "@Api";
import { useAuth } from "@hooks/authentication/useAuthentication";
import { format } from 'date-fns';
import { AxiosResponse } from "axios";
import toast from "react-hot-toast";

export const PlayPage = () => {
    const [pickedNumbers, setPickedNumbers] = useState<number[]>([]);
    const {user} = useAuth();
    const buttons = Array.from({ length: 16 }, (_, i) => i + 1);
    const [prices] = useAtom(PriceAtom);
    const [autoplay, setAutoplay] = useState<boolean>(false);
    const [timesToPlay, setTimesToPlay] = useState<number>(0);

    const totalPrice = () => {
        const count = pickedNumbers.length;

        if (count < 5) return 0;

        const priceObj = prices.find((p) => p.numbers === count);

        return priceObj?.price1 || 0;
    };

    const toggleNumber = (n: number) => {
        setPickedNumbers((prev) => {
            if (prev.includes(n)) {
                return prev.filter((num) => num !== n);
            } else if (prev.length < 8) {
                return [...prev, n];
            }
            return prev;
        });
    };

    const playBoard =() => {
        if (user != null){
            if (!autoplay)
            {
                const board : PlayBoardDTO = {
                    userid : user.id!,
                    numbers : pickedNumbers!,
                    dateofpurchase : format(new Date(), 'yyyy-MM-dd')
                };
                const api = new Api();
                api.board.boardPlayBoard(board).then((r: AxiosResponse<BoardResponseDTO>) =>{
                    toast.success("Your board has been played");
                });
            }else if(timesToPlay>0) {
                const board : PlayAutoplayBoardDTO = {
                    userid : user.id!,
                    numbers : pickedNumbers!,
                    leftToPlay : timesToPlay
                };
                const api = new Api();
                api.board.boardAutoplayBoard(board).then((r: AxiosResponse<BoardResponseDTO>) =>{
                    toast.success("You have successfully set an autoplay!");
                })
            }
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
            <div className="flex justify-center items-center">
                <div className="flex justify-between items-center w-72">
                    <label
                        className="flex justify-center lg:text-2xl text-xl ml-12">Price: {autoplay ? totalPrice() * timesToPlay : totalPrice()}</label>
                    <div className="flex justify-center items-center form-control">
                        <label className="label cursor-pointer w-32 ">
                            <span className="label-text text-xl">Autoplay</span>
                            <input
                                type="checkbox"
                                onChange={(e) => setAutoplay(e.target.checked)}
                                defaultChecked={false}
                                className="checkbox"
                            />

                        </label>
                    </div>
                </div>
            </div>
            <div className="flex justify-center">
                <div className="flex justify-center items-center form-control">
                    <label className="label w-72">
                        <span className="label-text text-xl">Times to play</span>
                        <input type="number" disabled={!autoplay} onChange={(e) => setTimesToPlay(parseInt(e.target.value) || 0)} defaultChecked
                               className="input input-bordered w-32" defaultValue={0}/>
                    </label>
                </div>
            </div>
            <div className="flex justify-center">
                <Button disabled={pickedNumbers.length < 5} className="btn btn-primary w-48 text-center text-xl"
                        onClick={() => playBoard()}>
                    Accept
                </Button>
            </div>
        </LargeContainer>
    );
};
