import {Api, AutoplayBoardDTO} from "@Api.ts";
import {useEffect, useState} from "react";
import {useAuth} from "@hooks/authentication/useAuthentication.ts";



export const AutoplayHistory = () => {

    const [data, setData] = useState<AutoplayBoardDTO[]>([]);
    const {user} = useAuth();
    const fetchData = async () => {
        try {
            if (user?.id != undefined){
                const api = new Api();
                const response = await api.board.boardGetAutoplayBoards(user.id);
                setData(response.data);
            }
        } catch (error) {
            console.error("Error fetching game history:", error);
        }
    };

    useEffect(() => {

            fetchData();

    },[]);

    return <>

            <div className="flex flex-col gap-5">
                <table className="w-full">
                    <thead>
                    <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                        <th className="rounded-l-xl text-xs text-start lg:text-center">Left to play</th>
                        <th className="text-xs rounded-r-xl text-start lg:text-center">Numbers</th>
                    </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block">
                    {data.map((value: AutoplayBoardDTO) => (
                        <tr
                            key={value.id}
                            className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm lg:align-middle"
                        >
                            <td className="lg:text-center lg:align-middle">{value.leftToPlay}</td>
                            <td className="lg:text-center lg:align-middle">
                                {value.numbers!.join(", ")}
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
    </>
}