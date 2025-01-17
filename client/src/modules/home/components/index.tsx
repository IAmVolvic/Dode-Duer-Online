import { LargeContainer } from "@components/containers"
import { ButtonColor, LinkButton } from "@components/inputs/button"

export const Home = () => {
    return (
        <LargeContainer className="flex flex-col gap-32 mb-52 my-40">
            <div className="flex flex-col">
                <h1 className="text-4xl lg:text-8xl font-bold">Døde Duer Online</h1>
                <p className="text-lg lg:text-xl">Welcome to Døde Duer Online, brought to you by Group B – Start playing and earn big!</p>
            </div>

            <div className="flex flex-row flex-wrap gap-8">
                <LinkButton color={ButtonColor.primary} title="Play Now" linkTo="/play" />
                <LinkButton color={ButtonColor.info} title="Contact Us" linkTo="/" />
            </div>
        </LargeContainer>
    )
}