import { useState } from 'react'; 
import { useNavigate } from "react-router-dom";
import { useAuthContext } from '../Context/AuthContext';
import { ILoginDTO } from '../Interfaces/Dtos/ILoginDTO';

const LoginPanel = () => {
    const { signIn } = useAuthContext();
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    const handleLogin = async () => {
        const loginData: ILoginDTO = { email, password };
        try {
            await signIn(loginData);
            navigate("/", { replace: true });
        } catch (error) {
            console.error("Error al iniciar sesión:", error);
        }
    };

    return (
        <div className="container d-flex justify-content-center align-items-center">
            <div className="card p-4 shadow-lg" style={{ width: "400px" }}>
                <h3 className="text-center mb-4">Iniciar sesión</h3>
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
                <button className="btn btn-primary w-100" onClick={handleLogin}>
                    Ingresar
                </button>
            </div>
        </div>
    );
};

export default LoginPanel;
