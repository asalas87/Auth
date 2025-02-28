import { useState } from 'react';
import { useAuthContext } from '../Context/AuthContext';
import { IRegisterDTO } from '../Interfaces/IRegisterDTO';

const RegisterPanel = () => {
    const { signUp } = useAuthContext();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");

    const handleRegister = async () => {
        const registerData: IRegisterDTO = { email, password, confirmPassword };
        try {
            await signUp(registerData);
            console.log("Registro exitoso");
        } catch (error) {
            console.error("Error al registrar usuario:", error);
        }
    };

    return (
        <div className="container d-flex justify-content-center align-items-center vh-100">
            <div className="card p-4 shadow-lg" style={{ width: "400px" }}>
                <h3 className="text-center mb-4">Registrarse</h3>
                <div className="mb-3">
                    <label htmlFor="email" className="form-label">Correo Electrónico</label>
                    <input
                        type="email"
                        id="email"
                        className="form-control"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="password" className="form-label">Contraseña</label>
                    <input
                        type="password"
                        id="password"
                        className="form-control"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>
                <div className="mb-3">
                    <label htmlFor="confirmPassword" className="form-label">Confirmar Contraseña</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        className="form-control"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                    />
                </div>
                <button className="btn btn-primary w-100" onClick={handleRegister}>
                    Registrarse
                </button>
            </div>
        </div>
    );
};

export default RegisterPanel;
