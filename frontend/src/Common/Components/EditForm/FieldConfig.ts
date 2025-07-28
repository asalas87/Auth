import { FieldType } from "./FieldType";

export type FieldConfig<T> = {
    name: keyof T;
    label: string;
    type: FieldType;
    options?: { value: string | number; label: string }[];
    events?: { [K in keyof React.DOMAttributes<any>]?: React.DOMAttributes<any>[K] };
};
