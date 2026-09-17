import { TextField } from '@mui/material';

type EmailFieldProps = Omit<React.ComponentProps<typeof TextField>, 'type'>;

export const EmailField = ({ ...props }: EmailFieldProps) => {
    return (
        <TextField
            {...props}
            type="email"
            size="small"
            fullWidth
        />
    );
};