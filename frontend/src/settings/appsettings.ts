export const appsettings = {
    turnstileSiteKey: import.meta.env.VITE_TURNSTILE_SITE_KEY || '1x00000000000000000000AA',
    apiUrl: import.meta.env.VITE_API_URL ?? "https://localhost:7277/"
}
