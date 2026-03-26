import { useEffect, useState, useCallback } from 'react';

export function useList<T>(
    fetchFn: () => Promise<T[]>
) {
    const [data, setData] = useState<T[]>([]);

    const loadData = useCallback(async () => {
            const result = await fetchFn();
            setData(result);
    }, [fetchFn]);

    useEffect(() => {
        loadData();
    }, [loadData]);

    return {
        data,
        reload: loadData
    };
}