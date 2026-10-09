import React from "react";

interface UserInfoProps {
    name?: string;
    company?: string;
}

const UserInfo = ({ name, company }: UserInfoProps) => (
    <div className="d-none d-sm-block text-end me-3">
        <div className="fw-semibold text-muted" style={{ fontSize: '0.9rem', lineHeight: '1.2' }}>
            {name || 'Usuario'}
        </div>
        <div style={{ fontSize: '0.75rem', color: '#6c757d' }}>
            {company || 'Empresa'}
        </div>
    </div>
);

export default UserInfo;