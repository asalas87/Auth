export interface ColumnConfig<T> {
    key: keyof T;
    label: string;
    render?: (item: any, row: T) => React.ReactNode;
}
