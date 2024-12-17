
export const BoardHistory = () => {

    return <>

            <div className="flex flex-col gap-5">
                <table className="w-full">
                    <thead>
                    <tr className="bg-base-300 h-12 hidden text-start lg:table-row">
                        <th className="rounded-l-xl text-xs">GameId</th>
                        <th className="text-xs text-start">Started</th>
                        <th className="text-xs text-start">Ended</th>
                        <th className="text-xs text-start">Status</th>
                        <th className="rounded-r-xl"></th>
                    </tr>
                    </thead>

                    <tbody className="before:content-['\200C'] before:leading-4 before:block">
                    {/* {data.map((value: AutoplayBoardDTO) => (
                        <tr
                            key={value.id}
                            className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm lg:align-middle"
                        >
                            <td className="lg:text-center lg:align-middle">{value.leftToPlay}</td>
                            <td className="lg:text-center lg:align-middle">
                                {value.numbers!.join(", ")}
                            </td>
                        </tr>
                    ))} */}
                    </tbody>
                </table>
            </div>
    </>
}