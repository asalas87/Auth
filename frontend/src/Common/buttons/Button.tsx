import { ButtonHTMLAttributes, ReactNode } from 'react';

type ButtonVariant = 'primary' | 'secondary' | 'outline-primary';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    children: ReactNode;
    variant?: ButtonVariant;
    loading?: boolean;
    loadingText?: string;
}

const Button = ({
    children,
    variant = 'primary',
    loading = false,
    loadingText = 'Cargando...',
    disabled,
    className = '',
    ...props
}: ButtonProps) => {
    return (
        <button
            type="button"
            className={`btn btn-${variant} ${className}`}
            disabled={disabled || loading}
            {...props}
        >
            {loading ? (
                <span className="d-inline-flex align-items-center justify-content-center gap-2">
                    <span
                        className="spinner-border spinner-border-sm"
                        role="status"
                        aria-hidden="true"
                    />
                    {loadingText}
                </span>
            ) : (
                children
            )}
        </button>
    );
};

export default Button;