import { useEffect, useRef, useState } from 'react';

const formatRemaining = (ms: number): string => {
    const totalSeconds = Math.floor(ms / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
};

export const useBlockCountdown = (
    blockedUntil: Date | null,
    onExpire: () => void,
): string | null => {
    const [remainingTime, setRemainingTime] = useState<string | null>(null);

    // Guardamos el callback en un ref para no re-disparar el efecto
    const onExpireRef = useRef(onExpire);
    useEffect(() => {
        onExpireRef.current = onExpire;
    }, [onExpire]);

    useEffect(() => {
        if (!blockedUntil) {
            setRemainingTime(null);
            return;
        }

        const tick = () => {
            const diff = blockedUntil.getTime() - Date.now();
            if (diff <= 0) {
                onExpireRef.current(); // llamamos a la versión actual
                return;
            }
            setRemainingTime(formatRemaining(diff));
        };

        tick();
        const interval = setInterval(tick, 1000);
        return () => clearInterval(interval);
    }, [blockedUntil]); // ← solo blockedUntil en deps

    return remainingTime;
};