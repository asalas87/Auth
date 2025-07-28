import React from 'react';
import { FieldType } from './FieldType';
import { FieldRenderer } from './FieldRenderer';

export interface FormField {
  name: string;
  label: string;
  type: FieldType;
  options?: { value: string | number; label: string }[]; // Solo para select
  required?: boolean;
  disabled?: boolean;
  placeholder?: string;
  min?: string;
  max?: string;
}

interface EditFormProps {
  fields: FormField[];
  formData: Record<string, any>;
  onChange: (fieldName: string, value: any) => void;
  onSubmit: (e: React.FormEvent<HTMLFormElement>) => void;
}

export const EditForm: React.FC<EditFormProps> = ({
  fields,
  formData,
  onChange,
  onSubmit,
}) => {
  return (
    <form onSubmit={onSubmit}>
      {fields.map((field) => (
        <div className="mb-3" key={field.name}>
          <label className="form-label">{field.label}</label>
          <FieldRenderer
            field={field}
            value={formData[field.name]}
            onChange={(value:string|number) => onChange(field.name, value)}
          />
        </div>
      ))}

      <button type="submit" className="btn btn-primary">
        Guardar
      </button>
    </form>
  );
};
