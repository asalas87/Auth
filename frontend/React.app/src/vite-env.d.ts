/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_API_URL: string;
    // Pod�s agregar m�s variables si us�s otras
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}

