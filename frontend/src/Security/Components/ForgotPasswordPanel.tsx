import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import TurnstileWidget from './TurnstileWidget';
import { useForgotPasswordFlow } from '../Hooks/useForgotPasswordFlow';
import { appsettings } from '@/settings/appsettings';
import { EmailField } from '@/Common/forms/EmailField';
import Button from '@/Common/buttons/Button';

const ForgotPasswordPanel = () => {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [sent, setSent] = useState(false);

    const {
        isSubmitting,
        captchaToken,
        turnstileRef,
        submit,
        onCaptchaSuccess,
        onCaptchaError,
    } = useForgotPasswordFlow();

    const handleSubmit = async () => {
        const ok = await submit(email);

        if (ok) {
            setSent(true);
        }
    };

    const isSubmitDisabled =
        isSubmitting ||
        !email ||
        !captchaToken;

    if (sent) {
        return (
            <div className="card p-4 shadow-lg text-center">
                <h3 className="mb-3">
                    Revisá tu correo
                </h3>

                <p className="text-muted mb-4">
                    Si el correo está registrado, recibirás un enlace para restablecer tu contraseña.
                </p>

                <Button
                    variant="outline-primary"
                    onClick={() => navigate('/auth', { replace: true })}
                >
                    Volver al inicio de sesión
                </Button>
            </div>
        );
    }

    return (
        <div className="card p-4 shadow-lg">
            <h3 className="text-center mb-4">
                Recuperar contraseña
            </h3>

            <p className="text-muted small mb-3">
                Ingresá tu correo y te enviaremos un enlace para restablecer tu contraseña.
            </p>

            <div className="mb-3">
                <EmailField
                    label="Correo Electrónico"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    disabled={isSubmitting}
                    sx={{
                        '& .MuiInputBase-root': {
                            height: 38,
                            fontSize: '0.9rem',
                        },
                    }}
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
                loadingText="Enviando..."
            >
                Enviar instrucciones
            </Button>

            <Link to="/auth" className="btn btn-link w-100 mt-2">
                Volver al inicio de sesión
            </Link>
        </div>
    );
};

export default ForgotPasswordPanel;