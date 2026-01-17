import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export default function RequirePermission({ permission, children }) {
  const { user, isAuthenticated, hasPermission } = useAuth();
  const location = useLocation();

  // ⏳ Estado transitorio mientras React sincroniza auth
  if (user === undefined) {
    return null; // o spinner si quieres
  }

  // 🔐 No autenticado
  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  // 🚫 Sin permiso
  if (permission && !hasPermission(permission)) {
    return <Navigate to="/403" replace />;
  }

  return children;
}
