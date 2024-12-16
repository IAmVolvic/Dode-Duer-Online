import {IBaseDialog} from "@components/dialogs";
import {GameResponseDTO} from "@Api.ts";

interface UserCreateDialogProps extends IBaseDialog {
    refresh: () => void;
}


export const BoardsShow = (props: UserCreateDialogProps) => {
    return <>
        <div className="flex flex-col gap-5">
            <table className="w-full">
                <thead>
                <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                    <th className="rounded-l-xl text-xs text-start lg:text-center">Player name</th>
                    <th className="text-xs text-start lg:text-center">Date</th>
                    <th className="text-xs rounded-r-xl text-start lg:text-center">Numbers</th>
                </tr>
                </thead>

                <tbody className="before:content-['\200C'] before:leading-4 before:block">
                {data.map((value: GameResponseDTO) => (
                    <tr
                        key={value.id}
                        className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm lg:align-middle"
                    >
                        <td className="lg:text-center lg:align-middle">{value.date}</td>
                        <td className="lg:text-center lg:align-middle">
                            {addDaysToDateOnly(value.date!, 6)}
                        </td>
                        <td className="lg:text-center lg:align-middle">
                            {value.status === 0 ? "Active" : "Inactive"}
                        </td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    </>
}