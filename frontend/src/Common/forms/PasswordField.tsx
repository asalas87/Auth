import { useState } from 'react';
import { TextField, IconButton, InputAdornment } from '@mui/material';
import Visibility from '@mui/icons-material/Visibility';
import VisibilityOff from '@mui/icons-material/VisibilityOff';

type PasswordFieldProps = Omit<React.ComponentProps<typeof TextField>, 'type' | 'slotProps'>;

export const PasswordField = ({ ...props }: PasswordFieldProps) => {
    const [showPassword, setShowPassword] = useState(false);

    const handleClickShowPassword = () => setShowPassword((show) => !show);
    const handleMouseDownPassword = (event: React.MouseEvent<HTMLButtonElement>) => {
        event.preventDefault();
    };

    return (
        <TextField
            {...props}
            type={showPassword ? 'text' : 'password'}
            size="small"
            fullWidth
            slotProps={{
                input: {
                    endAdornment: (
                        <InputAdornment position="end">
                            <IconButton
                                aria-label="toggle password visibility"
                                onClick={handleClickShowPassword}
                                onMouseDown={handleMouseDownPassword}
                                edge="end"
                                size="small"
                            >
                                {showPassword ? <VisibilityOff fontSize="small" /> : <Visibility fontSize="small" />}
                            </IconButton>
                        </InputAdornment>
                    ),
                },
            }}
        />
    );
};