import { useEffect, useRef, useState, useCallback } from 'react';
import { loadScript } from '@/Helpers/loadScript';
import { TurnstileOptions } from '../../types/turnstile';

const TURNSTILE_SCRIPT_URL = 'https://challenges.cloudflare.com/turnstile/v0/api.js';

interface UseTurnstileOptions {
    siteKey: string;
    onSuccess: (token: string) => void;
    onError?: () => void;
}

interface UseTurnstileReturn {
    containerRef: React.RefObject<HTMLDivElement>;
    scriptLoaded: boolean;
    scriptError: boolean;
    reset: () => void;
}

export const useTurnstile = ({
    siteKey,
    onSuccess,
    onError,
}: UseTurnstileOptions): UseTurnstileReturn => {
    const containerRef = useRef<HTMLDivElement>(null);
    const widgetIdRef = useRef<string | null>(null);
    const [scriptLoaded, setScriptLoaded] = useState(false);
    const [scriptError, setScriptError] = useState(false);

    const onSuccessRef = useRef(onSuccess);
    const onErrorRef = useRef(onError);

    useEffect(() => {
        onSuccessRef.current = onSuccess;
        onErrorRef.current = onError;
    }, [onSuccess, onError]);

    const reset = useCallback(() => {
        if (widgetIdRef.current && window.turnstile) {
            window.turnstile.reset(widgetIdRef.current);
        }
    }, []);

    useEffect(() => {
        loadScript(TURNSTILE_SCRIPT_URL)
            .then(() => setScriptLoaded(true))
            .catch(() => setScriptError(true));
    }, []);

    useEffect(() => {
        if (!scriptLoaded || !window.turnstile || !containerRef.current) return;

        widgetIdRef.current = window.turnstile.render(containerRef.current, {
            sitekey: siteKey,
            callback: (token: string) => onSuccessRef.current(token),
            'error-callback': () => onErrorRef.current?.(),
            'expired-callback': reset,
            theme: 'light',
        } satisfies TurnstileOptions);

        return () => {
            if (widgetIdRef.current && window.turnstile) {
                window.turnstile.remove(widgetIdRef.current);
                widgetIdRef.current = null;
            }
        };
    }, [scriptLoaded, siteKey, reset]);

    return { containerRef, scriptLoaded, scriptError, reset };
};