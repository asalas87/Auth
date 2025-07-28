import * as React from 'react';
import { FieldConfig } from './FieldConfig';
import { FieldType } from './FieldType';

interface GenericEditFormProps<T> {
  item: T;
  fields: FieldConfig<T>[];
  onClose: () => void;
  onSave: (item: T) => void;
  mode?: 'edit' | 'create';
}

export function FieldRenderer<T>({
  field,
  value,
  onChange,
  eventOverrides,
}: {
  field: FieldConfig<T>;
  value: any;
  onChange: (name: keyof T, value: any) => void;
  eventOverrides?: Partial<React.DOMAttributes<any>>;
}) {
  const {
    name,
    label,
    type,
    options,
  } = field;

  const {
    onChange: eventOnChange,
    onInput,
    onFocus,
    onBlur,
    onKeyDown,
    ...restEvents
  } = eventOverrides ?? {};

  const handleChange = (e: React.ChangeEvent<any>) => {
    let newValue: any = e.target.value;

    if (type === FieldType.Date) {
      newValue = e.target.value ? new Date(e.target.value) : undefined;
    } else if (type === FieldType.File) {
      newValue = e.target.files?.[0] ?? null;
    }

    if (eventOnChange) eventOnChange(e);
    else onChange(name, newValue);
  };

  const dateValue =
    value instanceof Date
      ? value.toISOString().slice(0, 10)
      : typeof value === 'string' && /^\d{4}-\d{2}-\d{2}$/.test(value)
        ? value
        : '';

  switch (type) {
    case FieldType.TextArea:
      return (
        <textarea
          name={String(name)}
          value={value ?? ''}
          onChange={handleChange}
          onInput={onInput}
          onFocus={onFocus}
          onBlur={onBlur}
          onKeyDown={onKeyDown}
          {...restEvents}
          className="form-control"
        />
      );

    case FieldType.Select:
      return (
        <select
          name={String(name)}
          value={value ?? ''}
          onChange={handleChange}
          onFocus={onFocus}
          onBlur={onBlur}
          onKeyDown={onKeyDown}
          {...restEvents}
          className="form-select"
        >
          <option value="">-- Seleccionar --</option>
          {options?.map(opt => (
            <option key={opt.value} value={opt.value}>
              {opt.label}
            </option>
          ))}
        </select>
      );

    case FieldType.File:
      return (
        <input
          type="file"
          name={String(name)}
          onChange={handleChange}
          onFocus={onFocus}
          onBlur={onBlur}
          onKeyDown={onKeyDown}
          {...restEvents}
          className="form-control"
        />
      );

    case FieldType.Date:
      return (
        <input
          type="date"
          name={String(name)}
          value={dateValue}
          onChange={handleChange}
          onInput={onInput}
          onFocus={onFocus}
          onBlur={onBlur}
          onKeyDown={onKeyDown}
          {...restEvents}
          className="form-control"
        />
      );

    default:
      return (
        <input
          type={type}
          name={String(name)}
          value={value ?? ''}
          onChange={handleChange}
          onInput={onInput}
          onFocus={onFocus}
          onBlur={onBlur}
          onKeyDown={onKeyDown}
          {...restEvents}
          className="form-control"
        />
      );
  }
}

export function GenericEditForm<T extends { id?: string }>({
  item,
  fields,
  onClose,
  onSave,
  mode = 'edit',
}: GenericEditFormProps<T>) {
  const [form, setForm] = React.useState<T>(item);

  const onChange = (name: keyof T, value: any) => {
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSave(form);
  };

  return (
    <div className="modal show d-block" style={{ backgroundColor: '#00000088' }}>
      <div className="modal-dialog">
        <form className="modal-content" onSubmit={handleSubmit}>
          <div className="modal-header">
            <h5 className="modal-title">{mode === 'create' ? 'Crear elemento' : 'Editar elemento'}</h5>
            <button type="button" className="btn-close" onClick={onClose} />
          </div>
          <div className="modal-body">
            {fields.map(field => (
              <div className="mb-3" key={String(field.name)}>
                <label className="form-label">{field.label}</label>
                <FieldRenderer
                  field={field}
                  value={form[field.name]}
                  onChange={onChange}
                  eventOverrides={field.events}
                />
              </div>
            ))}
          </div>
          <button className="btn btn-link primary me-2" onClick={handleSubmit}>
            Guardar
          </button>
          <button onClick={onClose} className="btn btn-link secondary">
            Cancelar
          </button>
        </form>
      </div>
    </div>
  );
}
export {};