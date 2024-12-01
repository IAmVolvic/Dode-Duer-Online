// Importing Images

export const Background = () => {
	return (
        <div className="BackgroundVolvic h-224 w-full flex justify-center items-center absolute -z-10">
            {/* Circle Shadow */}
            <div className="absolute BackgroundVolvicShape h-full w-full"></div>

            {/* Bottom Shadow */}
            <div className="absolute h-full w-full bg-gradient-to-t from-base-100 to-transparent"></div>
        </div>
    )
}