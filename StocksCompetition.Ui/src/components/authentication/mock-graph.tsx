import { useEffect, useState } from 'react'
import { ChartData } from 'chart.js'
import { Line } from 'react-chartjs-2'
import { useAuthenticationFormsContext } from '@context/authentication-forms-context'

const pointsCount = 100
const variation = 5
const variationMultiplier = 10

const animationDuration = 2750
const durationBetweenPoints = animationDuration / pointsCount

const options = {
    responsive: true,
    maintainAspectRatio: true,
    plugins: {
        legend: {
            display: false
        }
    },
    scales: {
        x: {
            grid: {
                display: false
            }
        },
        y: {
            ticks: {
                callback: (value: any) => '£' + Math.floor(value)
            },
            grid: {
                display: false
            }
        }
    },
    interaction: {
        mode: 'nearest' as 'nearest',
        intersect: false
    },
    layout: {
        padding: {
            left: 16,
            right: 16,
            top: 32,
            bottom: 16,
        }
    },
    animation: {
        x: {
            type: 'number',
            easing: 'linear',
            duration: durationBetweenPoints,
            from: NaN,
            delay: (context: any) => {
                if (context.type !== 'data' || context.xStarted) {
                    return 0
                }
                
                context.xStarted = true
                return context.index * durationBetweenPoints
            }
        }
    }
}

export const MockGraph = () => {
    const formData = useAuthenticationFormsContext()
    
    const [renderColour, setRenderColour] = useState(formData.displayColour)
    const [data, setData] = useState<ChartData<'line', number[]>>()
    
    const labels = new Array(pointsCount).fill('')
    
    const generateChartValues = (): number[] => {
        const generatedData: number[] = []
        let previous = 100
        
        for (let i = 0; i < pointsCount; i++) {
            previous += variation - Math.random() * variationMultiplier
            generatedData.push(previous)
        }
        
        return generatedData
    }

    const toDataSet = (colour: string, data: number[]): ChartData<'line', number[]> => ({
        labels,
        datasets: [{
            label: 'Account Value',
            data: data,
            borderColor: colour,
            backgroundColor: colour,
            pointRadius: 0
        }]
    })
    
    useEffect(() => {
        const generatedData = generateChartValues()
        setData(toDataSet(formData.displayColour, generatedData))
    }, [])
    
    useEffect(() => {
        const interval = setInterval(() => setRenderColour(formData.displayColour), 200)
        return () => clearInterval(interval)
    }, [formData])

    useEffect(() => {
        setData(toDataSet(renderColour, data?.datasets[0].data || generateChartValues()))
    }, [renderColour])
    
    return (
        <div className='mock-graph-container'>
            { data !== undefined && <Line className='graph' data={data} options={options} /> }
        </div>
    )
}