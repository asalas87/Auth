import { useState } from 'react';
import { Alert, Button, Snackbar } from '@mui/material';
import { useErrorReport } from '../Hooks/useErrorReport';
import ErrorReportModal from './ErrorReportModal';

const ErrorReportBanner = () => {
    const { pendingError, dismissError } = useErrorReport();
    const [showModal, setShowModal] = useState(false);

    return (
        <>
            <Snackbar
                open={!!pendingError}
                anchorOrigin={{ vertical: 'top', horizontal: 'right' }}
                autoHideDuration={null}
                sx={{ '&.MuiSnackbar-root': { top: 80 } }}
            >
                <Alert
                    severity="warning"
                    variant="filled"
                    sx={{ alignItems: 'center' }}
                    action={
                        <>
                            <Button color="inherit" size="small" onClick={() => setShowModal(true)}>
                                Reportar problema
                            </Button>
                            <Button color="inherit" size="small" onClick={dismissError}>
                                Cerrar
                            </Button>
                        </>
                    }
                >
                    Ocurrió un error inesperado.
                </Alert>
            </Snackbar>
            <ErrorReportModal open={showModal} onClose={() => setShowModal(false)} />
        </>
    );
};

export default ErrorReportBanner;
