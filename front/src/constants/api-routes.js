export const API_ROUTES = {
  AUTH: {
    LOGIN: "/api/auth/login",
    REGISTER: "/api/auth/register",
    REFRESH_TOKEN: "/api/auth/refresh-token",
    LOGOUT: "/api/auth/logout",
  },

  // 👤 Usuarios
  USERS: {
    GET_ALL: "/api/users",
    GET_BY_ID: (id) => `/api/users/${id}`,
    CREATE: "/api/users",
    UPDATE: (id) => `/api/users/${id}`,
    UPDATE_ROLES: (id) => `/api/users/${id}/roles`,
    DELETE: (id) => `/api/users/${id}`,
  },

  // 🔐 Roles (Admin)
  ROLES: {
    GET_ALL: "/api/roles",
    GET_BY_ID: (id) => `/api/roles/${id}`,
    CREATE: "/api/roles",
    UPDATE: (id) => `/api/roles/${id}`,
    DELETE: (id) => `/api/roles/${id}`,
  },
};
