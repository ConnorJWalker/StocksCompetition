import { useContext } from 'react'
import { SunIcon } from '@components/icons/sun-icon'
import { MoonIcon } from '@components/icons/moon-icon'
import { ThemeContext } from '@context/theme-context'

export const ThemeToggle = () => {
    const { theme, setTheme } = useContext(ThemeContext)
    
    return (
        <button className='theme-toggle' onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}>
            <span className='icon-container'>
                { theme === 'dark' ? <MoonIcon /> : <SunIcon /> }
            </span>
            <span className='text-container'>Toggle</span>
        </button>
    )
}