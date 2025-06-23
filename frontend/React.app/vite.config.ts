import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';
import { fileURLToPath } from 'url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

export default defineConfig({
    plugins: [react()],
    server: {
        https: {
            key: fs.readFileSync('./localhost-key.pem'),
            cert: fs.readFileSync('./localhost.pem'),
        },
        port: 5173,
        host: 'https://api.csingenieria.com.ar/',
        open: true,
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, 'src'),
        },
    }
});

