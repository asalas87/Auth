import React from "react";
import { FieldConfig } from "./FieldConfig";
import { FieldRenderer } from "./FieldRenderer";
import CrudTableErrorBoundary from "../ErrorBoundary/ErrorBoundary";

interface GenericEditFormProps<T> {
  item: T;
  fields: FieldConfig<T>[];
  onClose: () => void;
  onSave: (item: T) => void;
  mode?: 'edit' | 'create';
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
    <CrudTableErrorBoundary>
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
                  {field.customControl ? (
                    field.customControl
                  ) : (
                    <FieldRenderer
                      field={field}
                      value={form[field.name]}
                      onChange={onChange}
                      eventOverrides={field.events}
                    />
                  )}
                </div>
              ))}
            </div>
            <div className="d-flex justify-content-center gap-5 modal-footer">
              <button
                type="button"
                className="btn btn-secondary"
                onClick={onClose}
              >
                Cancelar
              </button>

              <button
                type="submit"
                className="btn btn-primary"
                onClick={handleSubmit}
              >
                Guardar
              </button>
            </div>
          </form>
        </div>
      </div>
    </CrudTableErrorBoundary>
  );
}