import { createContext } from 'react'

interface IThemeContext {
    theme: string | null
    setTheme: (newValue: string) => void
}

export const ThemeContext = createContext<IThemeContext>({
    theme: null,
    setTheme: () => {}
})
