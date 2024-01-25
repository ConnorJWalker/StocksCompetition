import { Line } from 'react-chartjs-2'
import { useEffect, useState } from 'react'
import { ChartData } from 'chart.js'

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
        },
        axis: {
            
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
    const [data, setData] = useState<ChartData<'line', number[]>>()
    
    useEffect(() => {
        const generatedData = []
        
        let previous = 100
        for (let i = 0; i < pointsCount; i++) {
            previous += variation - Math.random() * variationMultiplier
            generatedData.push(previous)
        }
        
        setData({
            labels: new Array(pointsCount).fill(''),
            datasets: [{
                label: 'Account Value',
                data: generatedData,
                borderColor: '#ff0000',
                backgroundColor: '#ff0000',
                pointRadius: 0
            }]
        })
    }, [])
    
    return (
        <div className='mock-graph-container'>
            { data !== undefined && <Line className='graph' data={data} options={options} /> }
        </div>
    )
}