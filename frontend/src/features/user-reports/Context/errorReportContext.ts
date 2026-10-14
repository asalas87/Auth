import { createContext } from 'react';

export interface PendingError {
    traceId: string;
    url: string;
}

export interface ErrorReportContextType {
    pendingError: PendingError | null;
    reportError: (error: PendingError) => void;
    dismissError: () => void;
}

export const ErrorReportContext = createContext<ErrorReportContextType | undefined>(undefined);
