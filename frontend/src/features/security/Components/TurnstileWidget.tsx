import { forwardRef, useImperativeHandle } from 'react';
import { useTurnstile } from '../Hooks/useTurnstile';

interface TurnstileWidgetProps {
    onSuccess: (token: string) => void;
    onError?: () => void;
    siteKey: string;
}

export interface TurnstileWidgetRef {
    reset: () => void;
}

const TurnstileWidget = forwardRef<TurnstileWidgetRef, TurnstileWidgetProps>(
    ({ onSuccess, onError, siteKey }, ref) => {
        const { containerRef, scriptLoaded, scriptError, reset } = useTurnstile({
            siteKey,
            onSuccess,
            onError,
        });

        useImperativeHandle(ref, () => ({ reset }), [reset]);

        if (scriptError) {
            return (
                <div className="alert alert-warning my-3">
                    No se pudo cargar el CAPTCHA. Verificá tu conexión e intentá de nuevo.
                </div>
            );
        }

        if (!scriptLoaded) {
            return (
                <div className="d-flex justify-content-center my-3">
                    <div className="spinner-border text-primary" role="status">
                        <span className="visually-hidden">Cargando CAPTCHA...</span>
                    </div>
                </div>
            );
        }

        return (
            <div
                ref={containerRef}
                className="d-flex justify-content-center my-3"
                data-permanent
            />
        );
    }
);

TurnstileWidget.displayName = 'TurnstileWidget';

export default TurnstileWidget;