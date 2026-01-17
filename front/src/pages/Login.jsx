import { useState } from "react";
import { loginRequest } from "../api/auth.api";
import { useAuth } from "../auth/AuthContext";
import { useNavigate } from "react-router-dom";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const { login } = useAuth();
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
  const resp = await loginRequest(email, password);

  console.log("LOGIN RESPONSE:", resp);
  console.log("TOKEN:", resp?.data?.token);

  login(resp.data.token);

  console.log("TOKEN EN STORAGE:", localStorage.getItem("token"));

  navigate("/dashboard");
} catch (err) {
  setError(err.message);
  console.error("Detalle tecnico del error:", err);
}

  };

  return (
    <div className="container mt-5">
      <div className="row justify-content-center">
        <div className="col-md-4">
          <div className="card shadow">
            <div className="card-body">
              <h4 className="text-center mb-4">Login</h4>

              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label>Email</label>
                  <input
                    type="email"
                    className="form-control"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    required
                  />
                </div>

                <div className="mb-3">
                  <label>Password</label>
                  <input
                    type="password"
                    className="form-control"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                  />
                </div>

                <button
                  type="submit"
                  className="btn btn-primary w-100"
                  disabled={loading}
                >
                  {loading ? "Logging in..." : "Login"}
                </button>

                {error && (
                  <div className="alert alert-danger mt-3" role="alert">
                    {error}
                  </div>
                )}
              </form>

              {/*  Enlace a Registro */}
              <div className="text-center mt-3">
                <a href="/register">
                  ¿No tienes cuenta? Regístrate
                </a>
              </div>

            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
