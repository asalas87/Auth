import React from 'react';
import { FieldType } from '.';
export function GenericEditForm({ item, fields, onClose, onSave, mode = 'edit', }) {
    const [form, setForm] = React.useState(item);
    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };
    const handleOptionChange = (e) => {
        const { name, value } = e.target;
        setForm(prev => ({ ...prev, [name]: value }));
    };
    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(form);
    };
    return (React.createElement("div", { className: "modal show d-block", style: { backgroundColor: '#00000088' } },
        React.createElement("div", { className: "modal-dialog" },
            React.createElement("form", { className: "modal-content", onSubmit: handleSubmit },
                React.createElement("div", { className: "modal-header" },
                    React.createElement("h5", { className: "modal-title" }, mode === 'create' ? 'Crear elemento' : 'Editar elemento'),
                    React.createElement("button", { type: "button", className: "btn-close", onClick: onClose }, " ")),
                React.createElement("div", { className: "modal-body" }, fields.map(({ name, label, type, options }) => {
                    const fieldName = String(name);
                    const value = form[name] ?? '';
                    return (React.createElement("div", { className: "mb-3", key: fieldName },
                        React.createElement("label", { className: "form-label" }, label),
                        type === FieldType.TextArea ? (React.createElement("textarea", { name: fieldName, value: String(value), onChange: handleChange, className: "form-control" })) : type === FieldType.Select ? (React.createElement("select", { name: fieldName, value: String(value), onChange: handleOptionChange, className: "form-select" },
                            React.createElement("option", { value: "" }, "-- Seleccionar --"),
                            options?.map((opt) => (React.createElement("option", { key: opt.value, value: opt.value }, opt.label))))) : type === FieldType.File ? (React.createElement("input", { type: "file", name: String(name), onChange: (e) => {
                                const file = e.target.files?.[0] ?? null;
                                setForm(prev => ({
                                    ...prev,
                                    [name]: file, // file debe estar en el tipo
                                }));
                            }, className: "form-control" })) : (React.createElement("input", { type: type, name: fieldName, value: String(value), onChange: handleChange, className: "form-control" }))));
                })),
                React.createElement("button", { className: "btn btn-link primary me-2", onClick: handleSubmit }, "Guardar"),
                React.createElement("button", { onClick: onClose, className: "btn btn-link secondary" }, "Cancelar")))));
}
