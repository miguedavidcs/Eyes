import { decodeToken } from "../utils/jwt";

export function getCurrentUser() {
  const token = localStorage.getItem("token");
  if (!token) return null;

  const decoded = decodeToken(token);
  if (!decoded) return null;

  // Expiración
  const currentTime = Date.now() / 1000;
  if (decoded.exp && decoded.exp < currentTime) {
    localStorage.removeItem("token");
    return null;
  }

  // Roles
  const rawRoles =
    decoded.role ||
    decoded.roles ||
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  const roles = Array.isArray(rawRoles)
    ? rawRoles
    : rawRoles
    ? [rawRoles]
    : [];

  // 🔑 PERMISOS (manejo correcto de múltiples claims iguales)
  const permissions = Array.isArray(decoded.permission)
    ? decoded.permission
    : decoded.permission
    ? [decoded.permission]
    : [];

  return {
    id: decoded.sub,
    email: decoded.email,
    fullName: decoded.fullName, 
    roles,
    permissions,
  };
}
