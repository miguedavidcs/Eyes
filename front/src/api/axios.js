import axios from "axios";
import { ERROR_MESSAGES, HTTP_MESSAGES } from "../constants/http-messages";
import { API_ROUTES } from "../constants/api-routes";

const http = axios.create({
  // ⚠️ AJUSTA esto según tu backend real
  // Si tu login es /auth/login → usa sin /api
  baseURL: "http://localhost:8080",
  headers: {
    "Content-Type": "application/json",
  },
});

// ===============================
// Request interceptor (JWT)
// ===============================
http.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

// ===============================
// Response interceptor (401 / 403)
// ===============================
http.interceptors.response.use(
  (response) => response,
  (error) => {
    const status = error.response?.status;
    const url = error.config?.url || "";

    // ⛔ NO tocar errores del login
    if (url.includes(API_ROUTES.AUTH.LOGIN)) {
      return Promise.reject(error);
    }

    // 🔐 401 → sesión inválida
    if (status === HTTP_MESSAGES.UNAUTHORIZED) {
      localStorage.clear();
      window.location.href = "/login";
      return Promise.reject(error);
    }

    // 🚫 403 → sin permisos
    if (status === HTTP_MESSAGES.FORBIDDEN) {
      window.location.href = "/403";
      return Promise.reject(error);
    }

    const serverMessage = error.response?.data?.message;

    error.message =
      serverMessage ||
      ERROR_MESSAGES[status] ||
      ERROR_MESSAGES.DEFAULT;

    if (import.meta.env.DEV) {
      console.error("HTTP ERROR:", error.message);
    }

    return Promise.reject(error);
  }
);

export default http;
