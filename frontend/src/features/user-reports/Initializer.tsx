import { useEffect } from 'react';
import { initApiErrorReporter } from '@/lib/api';
import { useErrorReport } from './Hooks/useErrorReport';

const ErrorReportInitializer = () => {
    const { reportError } = useErrorReport();

    useEffect(() => {
        initApiErrorReporter(reportError);
    }, [reportError]);

    return null;
};

export default ErrorReportInitializer;
