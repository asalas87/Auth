import { useEffect, useState, useCallback } from 'react';

export interface PagedResult<T> {
    items: T[];
    totalCount: number;
}

export function usePaginatedList<T>(
    fetchFn: (page: number, pageSize: number, filter: string) => Promise<PagedResult<T>>,
    pageSize: number = 10
) {
    const [data, setData] = useState<T[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);
    const [filter, setFilter] = useState('');

    const loadData = useCallback(async () => {
        const result = await fetchFn(currentPage, pageSize, filter);
        setData(result.items);
        setTotalCount(result.totalCount);
    }, [fetchFn, currentPage, pageSize, filter]);

    useEffect(() => {
        loadData();
    }, [loadData]);

    return {
        data,
        totalCount,
        currentPage,
        setCurrentPage,
        filter,
        setFilter,
        pageSize,
        reload: loadData 
    };
}
