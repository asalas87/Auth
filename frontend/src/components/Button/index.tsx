import { ButtonHTMLAttributes, ReactNode } from 'react';

type ButtonVariant = 'primary' | 'secondary' | 'outline-primary' | 'outline-secondary' | 'Primary' | 'Secondary' | 'Tertiary';
type ButtonSize = 'small' | 'medium' | 'large';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
    label?: ReactNode;
    children?: ReactNode;
    variant?: ButtonVariant;
    size?: ButtonSize;
    loading?: boolean;
    loadingText?: string;
}

const normalizeVariant = (variant: ButtonVariant) => {
    const value = variant.toLowerCase();

    if (value === 'primary') return 'primary';
    if (value === 'secondary') return 'secondary';
    if (value === 'tertiary' || value === 'outline-primary') return 'outline-primary';
    if (value === 'outline-secondary') return 'outline-secondary';

    return value;
};

const Button = ({
    type = 'button',
    children,
    label,
    variant = 'primary',
    size = 'medium',
    loading = false,
    loadingText = 'Cargando...',
    disabled,
    className = '',
    ...props
}: ButtonProps) => {
    const resolvedVariant = normalizeVariant(variant);
    const resolvedSize = size === 'small' ? 'btn-sm' : size === 'large' ? 'btn-lg' : '';
    const resolvedContent = loading ? (
        <span className="d-inline-flex align-items-center justify-content-center gap-2">
            <span className="spinner-border spinner-border-sm" role="status" aria-hidden="true" />
            <span>{loadingText}</span>
        </span>
    ) : (
        label ?? children ?? 'Enviar'
    );

    return (
        <button
            type={type}
            className={`btn btn-${resolvedVariant} ${resolvedSize} ${className}`.trim()}
            disabled={disabled || loading}
            {...props}
        >
            {resolvedContent}
        </button>
    );
};

export default Button;
