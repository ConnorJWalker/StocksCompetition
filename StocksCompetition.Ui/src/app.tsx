import { Chart as ChartJs, Legend, LinearScale, LineElement, PointElement, Tooltip, CategoryScale } from 'chart.js'
import { Authentication } from '@pages/authentication'

ChartJs.register(Legend, LinearScale, LineElement, PointElement, Tooltip, CategoryScale)

const App = () => {
    return <Authentication />
}

export default App
