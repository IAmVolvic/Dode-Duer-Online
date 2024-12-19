import { CustomSelect, Option } from "@components/inputs/multiSelect";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, GameResponseDTO, WinnersDto,} from "@Api";
import toast from "react-hot-toast";
import { useEffect, useState } from "react";
import { ErrorToast } from "@components/errorToast";
import { useGetWinnersOfGame } from "@hooks/game/useGetWinnersOfGame";


interface GameEditDialogProps extends IBaseDialog {
    game: GameResponseDTO;
    refreshMain: () => void;
}


export const GameEditDialog = (props: GameEditDialogProps) => {
    const [selectedGameId, setSelectedGameId] = useState<string>();
    const [winningNumbersIsSet, setWinningNumbersIsSet] = useState<boolean>(false);
    const [selectedItems, setSelectedItems] = useState<Option[]>([]);
    const { data, isLoading, refetch } = useGetWinnersOfGame(selectedGameId!);

    const API = new Api();

    const options = Array.from({ length: 16 }, (_, i) => ({
        label: (i + 1).toString(),
        value: (i + 1).toString()
    }));


    useEffect(() => {
        if (!props.game){ return; }

        refetch();
        setSelectedGameId(props.game.id!)
        console.log(props.game.winningNumbers!.length)
        if(props.game.winningNumbers!.length > 0){
            setWinningNumbersIsSet(true);
        }else{
            setWinningNumbersIsSet(false);
        }

    }, [props.game])


    const handleOnChange = (selectedItems: Option[]) => {
        setSelectedItems(selectedItems);
    }

    const handleWinningNumbers = () => {
        if(selectedItems.length > 3 || selectedItems.length < 3){
            toast.error("Must select 3 winning numbers")
            return
        }

        API.game.gameSetWinningNumbers({gameId: props.game.id?.toString()!, winningNumbers: selectedItems.map(item => Number(item.value))}).then(() => {
            toast.success("Winning numbers set successfully")
            setWinningNumbersIsSet(true);
        }).catch((error) => {
            ErrorToast(error)
        })
    }

    const handleWinners = () => {
        API.winners.winnersEstablishWinners(props.game.id!).then((res) => {
            refetch();
            props.refreshMain();
        }).catch((error) => {});
    }
    
    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="Game View" dialogSize={DialogSizeEnum.mediumFixed} >
                <div className="flex flex-col w-full h-full gap-12 p-5 overflow-y-auto">
                    <div className="flex flex-col gap-7">
                        <CustomSelect options={options} placeHolder="Select 3 Winning Numbers" selectClassName="w-full bg-base-300 p-3 rounded-xl" onSelectionChange={handleOnChange} />
                        
                        <div className="flex flex-row items-center gap-5">
                            <button onClick={handleWinningNumbers} className={`flex flex-row justify-center items-center gap-5 bg-primary text-primary-content rounded-xl w-full h-10 ${winningNumbersIsSet ? 'opacity-50':''}`} disabled={winningNumbersIsSet}> 
                                <div>Set winning numbers</div>
                            </button>

                            <button onClick={handleWinners} className={`flex flex-row justify-center items-center gap-5 bg-primary text-primary-content rounded-xl w-full h-10 ${!winningNumbersIsSet ? 'opacity-50':''}`} disabled={!winningNumbersIsSet}> 
                                <div>Establish The Winners</div>
                            </button>
                        </div>
                        
                    </div>
                    

                    <table className="w-full">
                        <thead>
                            <tr className="bg-base-300 h-12 hidden lg:table-row">
                                <th className="rounded-l-xl"></th>
                                <th className="text-xs text-start">Username</th>
                                <th className="text-xs text-start">Times Won</th>
                                <th className="text-xs text-start">Amount Won</th>
                                <th className="rounded-r-xl"></th>
                            </tr>
                        </thead>

                        <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                            {!isLoading && data && 
                                Object.values(data as WinnersDto[]).map((value: WinnersDto, index: number) => {
                                    return (
                                        <tr key={index} className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                            <td ><img src={`https://api.dicebear.com/9.x/adventurer/svg?seed=${value?.name}`} alt="user" className="bg-base-100/50 w-10 h-10 rounded-full" /></td>
                                            <td >{value.name}</td>
                                            <td >{value.numberOfWinningBoards}</td>
                                            <td >{value.prize}</td>
                                        </tr>
                                    );
                                })
                            }
                        </tbody>
                    </table>
                </div>
            </BaseDialog>
        </>
    )
}