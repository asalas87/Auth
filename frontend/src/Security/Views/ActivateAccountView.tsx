import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuthContext } from "../Context/AuthContext";
import { toast } from "react-toastify";

const ActivateAccountView = () => {
    const { activate } = useAuthContext();
    const navigate = useNavigate();

    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [loading, setLoading] = useState(false);
    const [token, setToken] = useState<string | null>(null);
    const [validToken, setValidToken] = useState<boolean | null>(null);

    useEffect(() => {
        const params = new URLSearchParams(window.location.search);
        const t = params.get("token");
        setToken(t);
    }, []);

    const handleActivate = async (e: React.FormEvent) => {
        e.preventDefault();
        if (!token) {
            toast.error("El enlace de activación no es válido.");
            return;
        }
        if (password !== confirmPassword) {
            toast.error("Las contraseñas no coinciden.");
            return;
        }

        try {
            setLoading(true);
            await activate({ token, password, confirmPassword });
            toast.success("Cuenta activada correctamente. ¡Ya puedes iniciar sesión!");
            navigate("/", { replace: true });
        } catch (error: any) {
            console.error("Error al activar cuenta:", error);
            if (error.response?.status === 410) {
                toast.warning("El enlace de activación ha expirado. Se enviará uno nuevo.");
                // Aquí podrías llamar a un servicio que regenere el token
            } else {
                toast.error("No se pudo activar la cuenta.");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="d-flex justify-content-center align-items-center vh-100 bg-light">
            <div className="card shadow-lg p-4" style={{ width: "400px" }}>
                <h3 className="text-center mb-4">Activar Cuenta</h3>
                <p className="text-muted text-center">
                    Crea tu contraseña para activar tu cuenta.
                </p>

                <form onSubmit={handleActivate}>
                    <div className="mb-3">
                        <label htmlFor="password" className="form-label">
                            Contraseña
                        </label>
                        <input
                            type="password"
                            id="password"
                            className="form-control"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            required
                        />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="confirmPassword" className="form-label">
                            Confirmar Contraseña
                        </label>
                        <input
                            type="password"
                            id="confirmPassword"
                            className="form-control"
                            value={confirmPassword}
                            onChange={(e) => setConfirmPassword(e.target.value)}
                            required
                        />
                    </div>

                    <button
                        type="submit"
                        className="btn btn-primary w-100 mt-3"
                        disabled={loading}
                    >
                        {loading ? "Activando..." : "Activar Cuenta"}
                    </button>
                </form>

                <div className="text-center mt-3">
                    <small>
                        ¿Ya tienes una cuenta?{" "}
                        <a href="/" className="text-decoration-none text-primary">
                            Inicia sesión
                        </a>
                    </small>
                </div>
            </div>
        </div>
    );
};

export default ActivateAccountView;