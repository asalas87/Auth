import React from 'react';
import { FieldType } from './FieldType';

export type FieldConfig<T> = {
    name: keyof T;
    label: string;
    type: FieldType;
}

interface GenericEditFormProps<T> {
    item: T;
    fields: FieldConfig<T>[];
    onClose: () => void;
    onSave: (item: T) => void;
    mode?: 'edit' | 'create'; // <-- NUEVO
}

export function GenericEditForm<T extends { id?: string }>({
    item,
    fields,
    onClose,
    onSave,
    mode = 'edit',
}: GenericEditFormProps<T>) {
    const [form, setForm] = React.useState<T>(item);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
        const { name, value } = e.target;
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
                        <h5 className="modal-title">
                            {mode === 'create' ? 'Crear elemento' : 'Editar elemento'}
                        </h5>
                        <button type="button" className="btn-close" onClick={onClose}></button>
                    </div>
                    <div className="modal-body">
                        {fields.map(({ name, label, type }) => (
                            <div className="mb-3" key={String(name)}>
                                <label>{label}</label>
                                {type === 'textarea' ? (
                                    <textarea
                                        name={String(name)}
                                        value={String(form[name] ?? '')}
                                        onChange={handleChange}
                                        className="form-control"
                                    />
                                ) : (
                                    <input
                                        type={type}
                                        name={String(name)}
                                        value={String(form[name] ?? '')}
                                        onChange={handleChange}
                                        className="form-control"
                                    />
                                )}
                            </div>
                        ))}
                    </div>
                    <div className="modal-footer">
                        <button type="submit" className="btn btn-primary">
                            {mode === 'create' ? 'Crear' : 'Guardar'}
                        </button>
                        <button type="button" className="btn btn-secondary" onClick={onClose}>
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}
