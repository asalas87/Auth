import * as React from 'react';
import { FieldConfig } from './FieldConfig';
import { FieldType } from './FieldType';

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
        <>
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
          {value && (
            <div className="mt-2 text-muted small">
              Archivo seleccionado: <strong>{value.name}</strong>
            </div>
          )}
        </>
      );

    case FieldType.Date:
      const dateValue = value ? value.toISOString().slice(0, 10) : '';
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

export {}