import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export default function Navbar() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();

  // ⛔ Si no hay usuario autenticado, no se muestra el navbar
  if (!user) return null;

  return (
    <nav className="navbar navbar-expand navbar-dark bg-dark px-3">
      {/* Marca */}
      <Link className="navbar-brand" to="/dashboard">
        App
      </Link>

      {/* Navegación */}
      <div className="navbar-nav me-auto">
        <Link className="nav-link" to="/dashboard">
          Dashboard
        </Link>

        {/* 🔒 Opciones solo para Admin */}
        {user.isAdmin && (
          <>
            <Link className="nav-link" to="/admin">
              Usuarios
            </Link>
            <Link className="nav-link" to="/roles">
              Roles
            </Link>
          </>
        )}
      </div>

      {/* Usuario + logout */}
      <div className="d-flex text-white align-items-center">
        <span className="me-3">{user.email}</span>
        <button
          className="btn btn-outline-light btn-sm"
          onClick={() => {
            logout();
            navigate("/login");
          }}
        >
          Logout
        </button>
      </div>
    </nav>
  );
}
