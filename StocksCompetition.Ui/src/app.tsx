import { Chart as ChartJs, Legend, LinearScale, LineElement, PointElement, Tooltip, CategoryScale } from 'chart.js'
import { Authentication } from '@pages/authentication'
import { ThemeProvider } from '@providers/theme-provider'

ChartJs.register(Legend, LinearScale, LineElement, PointElement, Tooltip, CategoryScale)

const App = () => {
    return (
        <ThemeProvider>
            <Authentication />
        </ThemeProvider>
    )
}

export default App
