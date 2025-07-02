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
    preview: {
        https: {
            key: './localhost-key.pem',
            cert: './localhost.pem'
        },
        port: 5173,
        proxy: {
            '/api': {  // Recomendado: prefijar rutas API
                target: 'https://localhost:7277',
                changeOrigin: true,
                secure: false
            }
        }
    },
    build: {
        outDir: 'dist',
        emptyOutDir: true,
        sourcemap: false,
    },
})
