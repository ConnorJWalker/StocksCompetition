import { defineConfig } from 'vite'
import * as path from 'path'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [ react() ],
    resolve: {
        alias: [
            { find: '@components', replacement: path.resolve(__dirname, 'src', 'components') },
            { find: '@pages', replacement: path.resolve(__dirname, 'src', 'pages') },
            { find: '@models', replacement: path.resolve(__dirname, 'src', 'models') },
            { find: '@utilities', replacement: path.resolve(__dirname, 'src', 'utilities') }
        ]
    }
})
