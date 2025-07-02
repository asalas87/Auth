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

    const handleOptionChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSave(form);
    };

    return (
        <div className="modal show d-block" style={{ backgroundColor: '#00000088' }
        }>
            <div className="modal-dialog" >
                <form className="modal-content" onSubmit={handleSubmit} >
                    <div className="modal-header" >
                        <h5 className="modal-title" >
                            {mode === 'create' ? 'Crear elemento' : 'Editar elemento'}
                        </h5>
                        < button type="button" className="btn-close" onClick={onClose} > </button>
                    </div>
                    < div className="modal-body" >
                        {fields.map(({ name, label, type, options }) => {
                            const fieldName = String(name);
                            const value = form[name] ?? '';

                            return (
                                <div className="mb-3" key={fieldName}>
                                    <label className="form-label">{label}</label>
                                    {type === FieldType.TextArea ? (
                                        <textarea
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={handleChange}
                                            className="form-control"
                                        />
                                    ) : type === FieldType.Select ? (
                                        <select
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={handleOptionChange}
                                            className="form-select"
                                        >
                                            <option value="">-- Seleccionar --</option>
                                            {options?.map((opt) => (
                                                <option key={opt.value} value={opt.value}>
                                                    {opt.label}
                                                </option>
                                            ))}
                                        </select>
                                        ) : type === FieldType.File ? (
                                            <input
                                                type="file"
                                                name={String(name)}
                                                onChange={(e) => {
                                                    const file = e.target.files?.[0] ?? null;
                                                    setForm(prev => ({
                                                        ...prev,
                                                        [name]: file as any, // file debe estar en el tipo
                                                    }));
                                                }}
                                                className="form-control"
                                            />
                                            ) : (
                                        <input
                                            type={type}
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={handleChange}
                                            className="form-control"
                                        />
                                    )}
                                </div>
                            );
                        })}
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
