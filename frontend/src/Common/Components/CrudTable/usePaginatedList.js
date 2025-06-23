import { useEffect, useState } from 'react';
export function usePaginatedList(fetchFn, pageSize = 10) {
    const [data, setData] = useState([]);
    const [totalCount, setTotalCount] = useState(0);
    const [currentPage, setCurrentPage] = useState(1);
    const [filter, setFilter] = useState('');
    useEffect(() => {
        const load = async () => {
            const result = await fetchFn(currentPage, pageSize, filter);
            setData(result.items);
            setTotalCount(result.totalCount);
        };
        load();
    }, [fetchFn, currentPage, pageSize, filter]);
    return {
        data,
        totalCount,
        currentPage,
        setCurrentPage,
        filter,
        setFilter,
        pageSize
    };
}
