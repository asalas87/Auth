import { FieldType } from ".";

export type FieldConfig<T> = {
    name: keyof T;
    label: string;
    type: FieldType;
    options?: { value: string; label: string }[];
};
