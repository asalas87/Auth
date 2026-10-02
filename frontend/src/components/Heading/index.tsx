import { ReactNode } from 'react';

interface HeadingProps {
    label?: ReactNode;
    children?: ReactNode;
    size?: string;
    weight?: string;
    color?: string;
    className?: string;
    as?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6';
}

const Heading = ({
    label,
    children,
    size = 'h4',
    weight = 'fw-bold',
    color = 'text-dark',
    className = '',
    as,
    ...props
}: HeadingProps) => {
    const Component = as ?? 'h2';
    const content = label ?? children;

    return (
        <Component
            className={[size, weight, color, className].filter(Boolean).join(' ')}
            {...props}
        >
            {content}
        </Component>
    );
};

export default Heading;
