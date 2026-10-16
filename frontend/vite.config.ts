import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import fs from 'fs';
import path from 'path';

const appVersion = fs
    .readFileSync(path.resolve(__dirname, '..', 'VERSION'), 'utf-8')
    .trim();

export default defineConfig({
    plugins: [react()],
    define: {
        APP_VERSION: JSON.stringify(appVersion),
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
        },
    },
    server: {
        https: {
            key: './localhost-key.pem',
            cert: './localhost.pem',
        },
    },
    build: {
        rolldownOptions: {
            output: {
                codeSplitting: true,
                manualChunks(id) {
                    if (id.includes('react')) return 'react';
                    if (id.includes('axios')) return 'axios';
                },
            },
        },
    },
});