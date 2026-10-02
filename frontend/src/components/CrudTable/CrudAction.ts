export interface CrudAction<T> {
  label: string;
  icon: string;
  color?: string;
  onClick: (row: T) => void;
}
