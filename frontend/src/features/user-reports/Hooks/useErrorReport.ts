import { useContext } from 'react';
import { ErrorReportContext, ErrorReportContextType } from '../Context/errorReportContext';

export const useErrorReport = (): ErrorReportContextType => {
    const context = useContext(ErrorReportContext);
    if (!context) {
        throw new Error('useErrorReport debe usarse dentro de un ErrorReportProvider');
    }
    return context;
};
