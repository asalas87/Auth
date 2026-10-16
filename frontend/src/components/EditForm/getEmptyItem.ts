import { FieldConfig } from './FieldConfig';
import { FieldType } from './FieldType';

export function getEmptyItem<T>(fields: FieldConfig<T>[]): T {
    const empty: Partial<T> = {};

    for (const field of fields) {
        switch (field.type) {
            case FieldType.Text:
            case FieldType.TextArea:
            case FieldType.Date:
                empty[field.name] = '' as any;
                break;
            case FieldType.Number:
                empty[field.name] = 0 as any;
                break;
            default:
                empty[field.name] = '' as any;
        }
    }

    return empty as T;
}
