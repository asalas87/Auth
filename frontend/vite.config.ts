import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'path'

export default defineConfig({
    plugins: [react()],
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'), // Ruta absoluta a /src
        },
    },
    server: {
        https: {
            key: './localhost-key.pem',
            cert: './localhost.pem'
        }
    },
    build: {
        outDir: 'dist',
        emptyOutDir: true,
        sourcemap: false,
        rollupOptions: {
            output: {
                manualChunks: {
                    pdfjs: ['pdfjs-dist/build/pdf'],
                    react: ['react', 'react-dom']
                }
            }
        }
    },
})
