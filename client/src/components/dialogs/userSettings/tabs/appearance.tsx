import { useAtom } from "jotai";
import { ThemeAtom } from "@atoms/ThemeAtom";
import themes from "daisyui/src/theming/themes";
import { Theme } from "daisyui";

export const AppearanceTabContent = () => {
    const [theme, setTheme] = useAtom(ThemeAtom);

    const handleThemeChange = (themeName: Theme) => {
        localStorage.setItem('theme', themeName);
        setTheme(themeName);
    }

    return (
        <>
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
                {(Object.keys(themes) as Array<Theme>).map((themeName) => (
                    <button key={themeName} onClick={() => handleThemeChange(themeName)} className={`${(theme == themeName) ? "border-2 border-success" : "border-2 border-transparent"} rounded-xl overflow-hidden`}>
                        <div className="mockup-window bg-base-300 outline-none rounded-none" data-theme={themeName}>
                            <div className="bg-base-200 flex justify-center px-7 py-7 capitalize">{themeName}</div>
                        </div>
                    </button>
                ))}
            </div>
        </>
    );
}