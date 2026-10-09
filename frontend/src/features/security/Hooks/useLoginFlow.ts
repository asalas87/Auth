import { useEffect, useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { useAuthContext } from '../Context/AuthContext';
import { TurnstileWidgetRef } from '../Components/TurnstileWidget';

interface UseLoginFlowReturn {
    isSubmitting: boolean;
    requiresCaptcha: boolean;
    captchaToken: string | null;
    isBlocked: boolean;
    remainingTime: string | null;
    turnstileRef: React.RefObject<TurnstileWidgetRef>;
    submit: (email: string, password: string) => Promise<void>;
    onCaptchaSuccess: (token: string) => void;
    onCaptchaError: () => void;
}

const formatRemaining = (ms: number): string => {
    const totalSeconds = Math.floor(ms / 1000);
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${minutes}:${seconds.toString().padStart(2, '0')}`;
};

export const useLoginFlow = (): UseLoginFlowReturn => {
    const { signIn } = useAuthContext();
    const navigate = useNavigate();

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [requiresCaptcha, setRequiresCaptcha] = useState(false);
    const [captchaToken, setCaptchaToken] = useState<string | null>(null);
    const [blockedUntil, setBlockedUntil] = useState<Date | null>(null);
    const [remainingTime, setRemainingTime] = useState<string | null>(null);
    const turnstileRef = useRef<TurnstileWidgetRef>(null);

    const isBlocked = blockedUntil !== null;

    useEffect(() => {
        if (!blockedUntil) {
            setRemainingTime(null);
            return;
        }

        const tick = () => {
            const diff = blockedUntil.getTime() - Date.now();
            if (diff <= 0) {
                setBlockedUntil(null);
                setRequiresCaptcha(false);
                setCaptchaToken(null);
                return;
            }
            setRemainingTime(formatRemaining(diff));
        };

        tick();
        const interval = setInterval(tick, 1000);
        return () => clearInterval(interval);
    }, [blockedUntil]);

    const resetCaptcha = () => {
        setCaptchaToken(null);
        turnstileRef.current?.reset();
    };

    const submit = async (email: string, password: string) => {
        if (isSubmitting || isBlocked) return;

        if (requiresCaptcha && !captchaToken) {
            toast.error('Por favor, completá el CAPTCHA.');
            return;
        }

        setIsSubmitting(true);
        try {
            await signIn({
                email,
                password,
                ...(captchaToken && { captchaToken }),
            });
            navigate('/', { replace: true });
        } catch (error: any) {
            const status = error?.response?.status;

            if (status === 428) {
                setRequiresCaptcha(true);
                if (captchaToken) resetCaptcha();
                return;
            }

            if (status === 429) {
                const data = error?.response?.data;
                const until = data?.blockedUntil ? new Date(data.blockedUntil) : null;
                setBlockedUntil(until ?? new Date(Date.now() + 15 * 60 * 1000));
                setRequiresCaptcha(false);
                setCaptchaToken(null);
                return;
            }

            if (requiresCaptcha) resetCaptcha();
        } finally {
            setIsSubmitting(false);
        }
    };

    return {
        isSubmitting,
        requiresCaptcha: requiresCaptcha && !isBlocked,
        captchaToken,
        isBlocked,
        remainingTime,
        turnstileRef,
        submit,
        onCaptchaSuccess: (token) => setCaptchaToken(token),
        onCaptchaError: () => setCaptchaToken(null),
    };
};