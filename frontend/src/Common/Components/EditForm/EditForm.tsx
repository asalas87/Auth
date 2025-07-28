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
                        {fields.map(({ name, label, type, options, events }) => {
                            const fieldName = String(name);
                            const value = form[name] ?? '';

                            // Eventos pasados por FieldConfig (eventos extra)
                            // Los desestructuro para evitar pisar onChange internos
                            // si no se pasan, se usan los internos que actualizan el state
                            const {
                                onChange: eventOnChange,
                                onInput,
                                onFocus,
                                onBlur,
                                onKeyDown,
                                ...restEvents
                            } = events ?? {};

                            // Para los inputs estándar y textarea, si no hay onChange en events uso el handleChange
                            // Para select idem con handleOptionChange
                            // Para file input manejo especial

                            if (type === FieldType.TextArea) {
                                return (
                                    <div className="mb-3" key={fieldName}>
                                        <label className="form-label">{label}</label>
                                        <textarea
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={eventOnChange ?? handleChange}
                                            onInput={onInput}
                                            onFocus={onFocus}
                                            onBlur={onBlur}
                                            onKeyDown={onKeyDown}
                                            {...restEvents}
                                            className="form-control"
                                        />
                                    </div>
                                );
                            } else if (type === FieldType.Select) {
                                return (
                                    <div className="mb-3" key={fieldName}>
                                        <label className="form-label">{label}</label>
                                        <select
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={eventOnChange ?? handleOptionChange}
                                            onFocus={onFocus}
                                            onBlur={onBlur}
                                            onKeyDown={onKeyDown}
                                            {...restEvents}
                                            className="form-select"
                                        >
                                            <option value="">-- Seleccionar --</option>
                                            {options?.map((opt) => (
                                                <option key={opt.value} value={opt.value}>
                                                    {opt.label}
                                                </option>
                                            ))}
                                        </select>
                                    </div>
                                );
                            } else if (type === FieldType.File) {
                                return (
                                    <div className="mb-3" key={fieldName}>
                                        <label className="form-label">{label}</label>
                                        <input
                                            type="file"
                                            name={fieldName}
                                            onChange={(e) => {
                                                const file = e.target.files?.[0] ?? null;
                                                setForm(prev => ({
                                                    ...prev,
                                                    [name]: file as any, // El tipo file debe estar contemplado
                                                }));
                                                // También disparamos el onChange pasado si existe
                                                if (eventOnChange) eventOnChange(e);
                                            }}
                                            onFocus={onFocus}
                                            onBlur={onBlur}
                                            onKeyDown={onKeyDown}
                                            {...restEvents}
                                            className="form-control"
                                        />
                                    </div>
                                );
                            } else {
                                // Inputs tipo text, number, date, email, password, etc
                                return (
                                    <div className="mb-3" key={fieldName}>
                                        <label className="form-label">{label}</label>
                                        <input
                                            type={type}
                                            name={fieldName}
                                            value={String(value)}
                                            onChange={eventOnChange ?? handleChange}
                                            onInput={onInput}
                                            onFocus={onFocus}
                                            onBlur={onBlur}
                                            onKeyDown={onKeyDown}
                                            {...restEvents}
                                            className="form-control"
                                        />
                                    </div>
                                );
                            }
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
