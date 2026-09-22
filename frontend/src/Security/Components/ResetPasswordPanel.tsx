import { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import TurnstileWidget from './TurnstileWidget';
import { useResetPasswordFlow } from '../Hooks/useResetPasswordFlow';
import { appsettings } from '@/settings/appsettings';
import { PasswordField } from '@/Common/forms/PasswordField';
import Button from '@/Common/buttons/Button';

const ResetPasswordPanel = () => {
    const navigate = useNavigate();
    const [params] = useSearchParams();
    const token = params.get('token') ?? '';

    const [password, setPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [done, setDone] = useState(false);

    const {
        isSubmitting,
        captchaToken,
        turnstileRef,
        submit,
        onCaptchaSuccess,
        onCaptchaError,
    } = useResetPasswordFlow();

    const passwordsMatch =
        password.length > 0 && password === confirmPassword;

    const isSubmitDisabled =
        isSubmitting ||
        !passwordsMatch ||
        !captchaToken ||
        !token;

    const handleSubmit = async () => {
        const ok = await submit(token, password, confirmPassword);

        if (ok) {
            setDone(true);
        }
    };

    if (!token) {
        return (
            <div className="card p-4 shadow-lg text-center">
                <h3 className="mb-3">Enlace inválido</h3>

                <p className="text-muted mb-4">
                    El enlace de recuperación no es válido o está incompleto.
                </p>

                <Button
                    onClick={() => navigate('/forgot-password')}
                >
                    Solicitar nuevo enlace
                </Button>
            </div>
        );
    }

    if (done) {
        return (
            <div className="card p-4 shadow-lg text-center">
                <h3 className="mb-3">Contraseña actualizada</h3>

                <p className="text-muted mb-4">
                    Tu contraseña se cambió correctamente. Ya podés iniciar sesión.
                </p>

                <Button
                    onClick={() => navigate('/auth', { replace: true })}
                >
                    Ir al inicio de sesión
                </Button>
            </div>
        );
    }

    return (
        <div className="card p-4 shadow-lg">
            <h3 className="text-center mb-4">
                Nueva contraseña
            </h3>

            <div className="mb-3">
                <PasswordField
                    label="Nueva contraseña"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    disabled={isSubmitting}
                    autoComplete="new-password"
                />
            </div>

            <div className="mb-3">
                <PasswordField
                    label="Confirmar contraseña"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    disabled={isSubmitting}
                    autoComplete="new-password"
                    error={!!confirmPassword && !passwordsMatch}
                    helperText={
                        confirmPassword && !passwordsMatch
                            ? 'Las contraseñas no coinciden'
                            : ''
                    }
                />
            </div>

            <TurnstileWidget
                ref={turnstileRef}
                siteKey={appsettings.turnstileSiteKey}
                onSuccess={onCaptchaSuccess}
                onError={onCaptchaError}
            />

            <Button
                className="mt-3"
                onClick={handleSubmit}
                disabled={isSubmitDisabled}
                loading={isSubmitting}
                loadingText="Guardando..."
            >
                Cambiar contraseña
            </Button>
        </div>
    );
};

export default ResetPasswordPanel;