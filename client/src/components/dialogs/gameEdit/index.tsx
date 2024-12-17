import { CustomSelect, Option } from "@components/inputs/multiSelect";
import { BaseDialog, DialogSizeEnum, IBaseDialog } from "..";
import { Api, GameResponseDTO,} from "@Api";
import toast from "react-hot-toast";
import { useState } from "react";


interface GameEditDialogProps extends IBaseDialog {
    game: GameResponseDTO;
    refresh: () => void;
}


export const GameEditDialog = (props: GameEditDialogProps) => {
    const [selectedItems, setSelectedItems] = useState<Option[]>([]);
    
    const API = new Api();

    const options = Array.from({ length: 16 }, (_, i) => ({
        label: (i + 1).toString(),
        value: (i + 1).toString()
    }));


    const handleOnChange = (selectedItems: Option[]) => {
        setSelectedItems(selectedItems);
    }

    const handleWinningNumbers = () => {
        if(selectedItems.length > 3 || selectedItems.length < 3){
            toast.error("Must select 3 winning numbers")
            return
        }

        API.game.gameSetWinningNumbers({gameId: props.game.id?.toString()!, winningNumbers: selectedItems.map(item => Number(item.value))})
    }
    
    return (
        <>
            <BaseDialog isOpen={props.isOpen} close={props.close} dialogTitle="User Edit" dialogSize={DialogSizeEnum.mediumFixed} >
                <div className="flex flex-col w-full h-full gap-16 p-5 overflow-y-auto">
                    <div className="flex flex-col gap-7">
                        <CustomSelect options={options} placeHolder="Select 3 Winning Numbers" selectClassName="w-full bg-base-300 p-3 rounded-xl" onSelectionChange={handleOnChange} />
                        
                        <button onClick={handleWinningNumbers} className="flex flex-row justify-center items-center gap-5 bg-primary text-primary-content rounded-xl w-full h-10"> 
                            <div>Set winning numbers</div>
                        </button>
                    </div>
                    

                    <table className="w-full">
                        <thead>
                            <tr className="bg-base-300 h-12 hidden lg:table-row">
                                <th className="rounded-l-xl"></th>
                                <th className="text-xs text-start">Username</th>
                                <th className="text-xs text-start">Winning Numbers</th>
                                <th className="text-xs text-start">Amount Won</th>
                                <th className="rounded-r-xl"></th>
                            </tr>
                        </thead>

                        <tbody className="before:content-['\200C'] before:leading-4 before:block ">

                            {/* {!isLoading && data && 
                                Object.values(data as GameResponseDTO[]).map((value: GameResponseDTO) => {
                                    return (
                                        <tr key={value.id} className="flex flex-col gap-2 pb-5 lg:h-16 lg:table-row border-b-0.05r border-base-content/50 text-sm">
                                            <td >{value.id}</td>
                                            <td >{value.date}</td>
                                            <td >{value.status}</td>
                                            <td >
                                                <button onClick={() => handleGameEdit(value)} className="flex justify-center items-center bg-primary text-primary-content rounded-xl w-12 h-7"> 
                                                    <FiEdit /> 
                                                </button>
                                            </td>
                                        </tr>
                                    );
                                })
                            } */}
                        </tbody>
                    </table>
                </div>
            </BaseDialog>
        </>
    )
}