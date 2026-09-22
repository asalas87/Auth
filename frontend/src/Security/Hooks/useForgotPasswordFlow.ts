import { useRef, useState } from 'react';
import { toast } from 'react-toastify';
import { forgotPassword } from '../Services/AccountService';
import { useBlockCountdown } from './useBlockCountdown';
import type { TurnstileWidgetRef } from '../Components/TurnstileWidget';

interface UseForgotPasswordFlowReturn {
    isSubmitting: boolean;
    requiresCaptcha: boolean;
    captchaToken: string | null;
    isBlocked: boolean;
    remainingTime: string | null;
    turnstileRef: React.RefObject<TurnstileWidgetRef>;
    submit: (email: string) => Promise<boolean>;
    onCaptchaSuccess: (token: string) => void;
    onCaptchaError: () => void;
}

export const useForgotPasswordFlow = (): UseForgotPasswordFlowReturn => {
    const [isSubmitting, setIsSubmitting] = useState(false);
    const requiresCaptcha = true;
    const [captchaToken, setCaptchaToken] = useState<string | null>(null);
    const [blockedUntil, setBlockedUntil] = useState<Date | null>(null);
    const turnstileRef = useRef<TurnstileWidgetRef>(null);

    const isBlocked = blockedUntil !== null;

    const remainingTime = useBlockCountdown(blockedUntil, () => {
        setBlockedUntil(null);
        setCaptchaToken(null);
    });

    const resetCaptcha = () => {
        setCaptchaToken(null);
        turnstileRef.current?.reset();
    };

    const submit = async (email: string): Promise<boolean> => {
        if (isSubmitting || isBlocked) return false;
        if (!captchaToken) {
            toast.error('Por favor, completá el CAPTCHA.');
            return false;
        }

        setIsSubmitting(true);
        try {
            await forgotPassword({ email, captchaToken });
            return true;
        } catch (error: any) {
            const status = error?.response?.status;

            if (status === 429) {
                const until = error?.response?.data?.blockedUntil;
                setBlockedUntil(
                    until ? new Date(until) : new Date(Date.now() + 15 * 60 * 1000),
                );
                return false;
            }

            resetCaptcha();
            toast.error(
                error?.response?.data?.message ??
                'No se pudo enviar la solicitud. Intentá nuevamente.',
            );
            return false;
        } finally {
            setIsSubmitting(false);
        }
    };

    return {
        isSubmitting,
        requiresCaptcha,
        captchaToken,
        isBlocked,
        remainingTime,
        turnstileRef,
        submit,
        onCaptchaSuccess: (token) => setCaptchaToken(token),
        onCaptchaError: () => setCaptchaToken(null),
    };
};