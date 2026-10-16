import { useState } from 'react';
import {
    Button,
    CircularProgress,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    TextField,
    Typography,
} from '@mui/material';
import { sendUserReport } from '../Services/userReportService';
import { useErrorReport } from '../Hooks/useErrorReport';

interface ErrorReportModalProps {
    open: boolean;
    onClose: () => void;
}

const ErrorReportModal = ({ open, onClose }: ErrorReportModalProps) => {
    const { pendingError, dismissError } = useErrorReport();
    const [message, setMessage] = useState('');
    const [submitting, setSubmitting] = useState(false);
    const [submissionError, setSubmissionError] = useState('');

    const handleSubmit = async () => {
        if (!message.trim() || !pendingError || submitting) {
            return;
        }

        setSubmitting(true);
        setSubmissionError('');

        try {
            await sendUserReport({
                message,
                traceId: pendingError.traceId,
                url: pendingError.url,
                userAgent: navigator.userAgent,
            });
            dismissError();
            onClose();
            setMessage('');
        } catch {
            setSubmissionError('No se pudo enviar el reporte. Intentá nuevamente.');
        } finally {
            setSubmitting(false);
        }
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle>Reportar problema</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    label="Descripción del problema"
                    multiline
                    rows={5}
                    fullWidth
                    required
                    value={message}
                    onChange={event => setMessage(event.target.value)}
                    slotProps={{ htmlInput: { maxLength: 5000 } }}
                    helperText={`${message.length}/5000`}
                />
                {submissionError && (
                    <Typography color="error" variant="body2" role="alert">
                        {submissionError}
                    </Typography>
                )}
                {pendingError?.traceId && (
                    <Typography
                        variant="caption"
                        color="text.secondary"
                        sx={{ display: 'block', mt: 2, fontFamily: 'monospace' }}
                    >
                        TraceId: {pendingError.traceId}
                    </Typography>
                )}
            </DialogContent>
            <DialogActions>
                <Button onClick={onClose} disabled={submitting}>
                    Cancelar
                </Button>
                <Button
                    onClick={handleSubmit}
                    disabled={submitting || !message.trim()}
                    variant="contained"
                >
                    {submitting ? <CircularProgress size={18} color="inherit" /> : 'Enviar'}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ErrorReportModal;
