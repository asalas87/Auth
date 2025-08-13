import { ColumnConfig } from "./ColumnConfig";

export interface CrudTableProps<T> {
    data: T[];
    columns: ColumnConfig<T>[];
    onEdit: (item: T) => void;
    onDelete: (item: T) => void;
    onFilter?: (filter: string) => void;
    onPageChange?: (page: number) => void;
    onNew?: () => void;
    pageSize?: number;
    currentPage?: number;
    showActions?: boolean;
    totalCount: number;
    newLabel?: string;
}