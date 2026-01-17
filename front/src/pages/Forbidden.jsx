import { Link } from "react-router-dom";

export default function Forbidden() {
  return (
    <div className="container mt-5 text-center">
      <h1 className="display-4 text-danger">403</h1>

      <p className="lead mt-3">
        No tienes permisos para acceder a esta sección.
      </p>

      <p className="text-muted">
        Si crees que esto es un error, contacta al administrador del sistema.
      </p>

      <div className="mt-4">
        <Link to="/dashboard" className="btn btn-primary me-2">
          Volver al Dashboard
        </Link>

        <Link to="/login" className="btn btn-outline-secondary">
          Ir al Login
        </Link>
      </div>
    </div>
  );
}
