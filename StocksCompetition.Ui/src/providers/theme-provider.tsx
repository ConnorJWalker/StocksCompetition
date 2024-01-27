import { ReactNode, useEffect, useRef, useState } from 'react'
import { ThemeContext } from '@context/theme-context'

interface props { children: ReactNode }

export const ThemeProvider = ({ children }: props) => {
    const [theme, setTheme] = useState('')
    const fromBrowserPreferences = useRef(true)
    
    useEffect(() => {
        const storedTheme = localStorage.getItem('theme')
        if (storedTheme !== null) {
            setTheme(storedTheme)
            fromBrowserPreferences.current = false
            
            return
        }

        const onThemeChange = (e: MediaQueryListEvent) => setTheme(e.matches ? 'light' : 'dark')
        const themeMediaQuery = window.matchMedia('(prefers-color-scheme: light)')
        themeMediaQuery.onchange = onThemeChange
        setTheme(themeMediaQuery.matches ? 'light' : 'dark')

        return () => themeMediaQuery.removeEventListener('change', onThemeChange)
    }, [])

    useEffect(() => {
        document.body.dataset.theme = theme
        if (!fromBrowserPreferences.current && theme !== '') {
            localStorage.setItem('theme', theme)
        }
    }, [theme])
    
    const setOverrideTheme = (newValue: string) => {
        fromBrowserPreferences.current = false
        setTheme(newValue)
    }
    
    return (
        <ThemeContext.Provider value={{ theme, setTheme: setOverrideTheme }}>
            { children }
        </ThemeContext.Provider>
    )
}