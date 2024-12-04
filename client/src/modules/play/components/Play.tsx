import { LargeContainer } from "@components/containers";

import { ButtonColor, LinkButton } from "@components/inputs/button"
import {Button} from "@headlessui/react";
import {useState} from "react";

export const PlayPage = () => {
    const buttons = Array.from({ length: 16 }, (_, i) => i + 1);
    const [pickedNumbers,setPickedNumbers] = useState<Number[]>([]);
    return (
        <LargeContainer className="flex flex-col gap-32 mb-52">
            <div className="flex justify-center">
                <h1 className="text-4xl sm:text-8xl font-bold">Play Board</h1>
            </div>
            <div className="flex flex-row flex-wrap gap-8 justify-center">
            {buttons.map((button) => (
                    <Button className="btn btn-neutral w-32 text-center text-lg">{button}</Button>
                ))}
            </div>
            <div className="flex justify-center">
                <Button className="btn btn-primary w-48 text-center text-xl">Accept</Button>
            </div>
        </LargeContainer>
    );
};
