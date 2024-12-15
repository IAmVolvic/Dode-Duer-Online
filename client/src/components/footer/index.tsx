import { SimpleContainer } from "@components/containers"

export const Footer = () => {
    const s = false
    const isLoading = s;
    const response = { failed: s };

    return (
        <div className="bg-transparent w-full h-36">
            <SimpleContainer className="flex flex-row justify-between text-neutral-content">
                <div className="text-2xl text-base-content font-medium"> Game Status </div>
                <div className="flex flex-row items-center gap-5">
                    <div className={`w-5 h-5 rounded-full status-ring ${isLoading || response?.failed ? 'status-ring-offline' : 'status-ring-online'}`}></div>
                    <div className="text-2xl text-base-content font-medium">{isLoading || response?.failed ? 'Closed' : 'Running'} - Week 46</div>
                </div>
            </SimpleContainer>
        </div>
    )
}