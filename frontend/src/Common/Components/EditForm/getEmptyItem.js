import { FieldType } from './FieldType';
export function getEmptyItem(fields) {
    const empty = {};
    for (const field of fields) {
        switch (field.type) {
            case FieldType.Text:
            case FieldType.TextArea:
            case FieldType.Date:
                empty[field.name] = '';
                break;
            case FieldType.Number:
                empty[field.name] = 0;
                break;
            default:
                empty[field.name] = '';
        }
    }
    return empty;
}
