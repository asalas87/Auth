import { useEffect, useState } from 'react';
export function useSimulatedProgress(active) {
    const [progress, setProgress] = useState(0);
    useEffect(() => {
        let timer;
        if (active) {
            setProgress(0);
            timer = setInterval(() => {
                setProgress(prev => (prev < 90 ? prev + 10 : prev));
            }, 200);
        }
        else {
            setProgress(100);
            setTimeout(() => setProgress(0), 300); // reset despu�s de finalizar
        }
        return () => clearInterval(timer);
    }, [active]);
    return progress;
}
