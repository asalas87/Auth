import { ColumnConfig } from "./ColumnConfig";
import { CrudAction } from "./CrudAction";

export interface CrudTableProps<T> {
    data: T[];
    columns: ColumnConfig<T>[];
    actions?: CrudAction<T>[];
    onEdit?: (item: T) => void;
    onDelete?: (item: T) => void;
    onFilter?: (filter: string) => void;
    onPageChange?: (page: number) => void;
    onNew?: () => void;
    pageSize?: number;
    currentPage?: number;
    showActions?: boolean;
    totalCount: number;
    newLabel?: string;
}