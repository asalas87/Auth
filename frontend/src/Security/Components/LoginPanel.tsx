import { useState } from 'react';
import TurnstileWidget from './TurnstileWidget';
import { useLoginFlow } from '../Hooks/useLoginFlow';
import { appsettings } from '../../settings/appsettings';
import { PasswordField } from '@/Common/forms/PasswordField';
import { EmailField } from '@/Common/forms/EmailField';

const LoginPanel = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

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
    const isSubmitDisabled = isSubmitting || (requiresCaptcha && !captchaToken);

    return (
        <div className="container d-flex justify-content-center align-items-center">
            <div className="card p-4 shadow-lg" style={{ width: "400px" }}>
                <h3 className="text-center mb-4">Iniciar sesión</h3>

                <div className="mb-3">
                    <EmailField
                        label="Correo Electrónico"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        disabled={isFormDisabled}
                        sx={{
                            '& .MuiInputBase-root': {
                                height: '38px',  // altura específica
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
                            if (e.key === 'Enter' && !isSubmitDisabled) handleSubmit();
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

                <button
                    type="button"
                    className="btn btn-primary w-100 d-flex align-items-center justify-content-center gap-2"
                    onClick={handleSubmit}
                    disabled={isSubmitDisabled}
                >
                    {isSubmitting ? (
                        <>
                            <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true" />
                            Ingresando...
                        </>
                    ) : (
                        'Ingresar'
                    )}
                </button>
            </div>
        </div>
    );
};

export default LoginPanel;