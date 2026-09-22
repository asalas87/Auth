import { useState } from 'react';
import TurnstileWidget from './TurnstileWidget';
import { useLoginFlow } from '../Hooks/useLoginFlow';
import { appsettings } from '../../settings/appsettings';
import { PasswordField } from '@/Common/forms/PasswordField';
import { EmailField } from '@/Common/forms/EmailField';
import Button from '@/Common/buttons/Button';
import { Link } from 'react-router-dom';

const LoginPanel = () => {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const {
        isSubmitting,
        requiresCaptcha,
        captchaToken,
        turnstileRef,
        submit,
        onCaptchaSuccess,
        onCaptchaError,
    } = useLoginFlow();

    const handleSubmit = () => submit(email, password);

    const isFormDisabled = isSubmitting;

    const isSubmitDisabled =
        isSubmitting ||
        (requiresCaptcha && !captchaToken);

    return (
        <div className="card p-4 shadow-lg">
            <h3 className="text-center mb-4">
                Iniciar sesión
            </h3>

            <div className="mb-3">
                <EmailField
                    label="Correo Electrónico"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    disabled={isFormDisabled}
                    sx={{
                        '& .MuiInputBase-root': {
                            height: '38px',
                            fontSize: '0.9rem',
                        },
                    }}
                />
            </div>

            <div className="mb-3">
                <PasswordField
                    label="Contraseña"
                    variant="outlined"
                    fullWidth
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    disabled={isFormDisabled}
                    onKeyDown={(e) => {
                        if (e.key === 'Enter' && !isSubmitDisabled) {
                            handleSubmit();
                        }
                    }}
                />
            </div>

            {requiresCaptcha && (
                <TurnstileWidget
                    ref={turnstileRef}
                    siteKey={appsettings.turnstileSiteKey}
                    onSuccess={onCaptchaSuccess}
                    onError={onCaptchaError}
                />
            )}

            <Button
                onClick={handleSubmit}
                disabled={isSubmitDisabled}
                loading={isSubmitting}
                loadingText="Ingresando..."
            >
                Ingresar
            </Button>

            <Link to="/forgot-password" className="btn btn-link w-100 mt-2">
                ¿Olvidaste tu contraseña?
            </Link>
        </div>
    );
};

export default LoginPanel;