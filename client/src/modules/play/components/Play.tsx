import { LargeContainer } from "@components/containers";
import { Button } from "@headlessui/react";
import { useState } from "react";

export const PlayPage = () => {
    const [pickedNumbers, setPickedNumbers] = useState<Number[]>([]);
    const buttons = Array.from({ length: 16 }, (_, i) => i + 1);

    const toggleNumber = (n: Number) => {
        setPickedNumbers((prev) =>
            prev.includes(n) ? prev.filter((num) => num !== n) : [...prev, n]
        );
    };

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
                    >
                        {button}
                    </Button>
                ))}
            </div>
            <div className="flex justify-center">
                <Button className="btn btn-primary w-48 text-center text-xl">
                    Accept
                </Button>
            </div>
        </LargeContainer>
    );
};
