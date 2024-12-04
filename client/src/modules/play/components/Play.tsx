import { LargeContainer } from "@components/containers";

import { ButtonColor, LinkButton } from "@components/inputs/button"

export const PlayPage = () => {
    const buttons = Array.from({ length: 16 }, (_, i) => i + 1);

    return (
        <LargeContainer className="flex flex-col gap-32 mb-52">
            <div className="flex flex-row flex-wrap gap-8">
                {buttons.map((button) => (
                    <button className="w-">{button}</button>
                ))}
            </div>
        </LargeContainer>
    );
};
