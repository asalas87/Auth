import { Outlet } from 'react-router-dom';

const AuthLayout = () => {
    return (
        <div className="min-vh-100 d-flex justify-content-center align-items-center p-3">
            <div className="w-100" style={{ maxWidth: 400 }}>
                <Outlet />
            </div>
        </div>
    );
};

export default AuthLayout;