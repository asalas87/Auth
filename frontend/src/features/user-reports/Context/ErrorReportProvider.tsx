import { useCallback, useState } from 'react';
import { ErrorReportContext, PendingError } from './errorReportContext';

export const ErrorReportProvider = ({ children }: { children: React.ReactNode }) => {
    const [pendingError, setPendingError] = useState<PendingError | null>(null);

    const reportError = useCallback((error: PendingError) => {
        setPendingError(previous => previous ?? error);
    }, []);

    const dismissError = useCallback(() => {
        setPendingError(null);
    }, []);

    return (
        <ErrorReportContext.Provider value={{ pendingError, reportError, dismissError }}>
            {children}
        </ErrorReportContext.Provider>
    );
};
