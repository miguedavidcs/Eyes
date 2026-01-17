import { createContext, useContext, useEffect, useState } from "react";
import { getCurrentUser } from "./auth.service";

const AuthContext = createContext(null);

export function AuthProvider({ children }) {
  const [user, setUser] = useState(undefined); // 👈 clave
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const currentUser = normalizeUser(getCurrentUser());
    setUser(currentUser);
    setLoading(false);
  }, []);

  const login = (token) => {
    localStorage.setItem("token", token);
    const currentUser = normalizeUser(getCurrentUser());
    console.log("AUTH USER (after login):", currentUser);
    setUser(currentUser);
  };

  const logout = () => {
    localStorage.clear();
    setUser(null);
  };

  const isAuthenticated = !!user;

  const hasPermission = (permission) => {
    if (!user) return false;
    return user.permissions?.includes(permission);
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        loading,
        login,
        logout,
        isAuthenticated,
        hasPermission,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export const useAuth = () => useContext(AuthContext);

function normalizeUser(rawUser) {
  if (!rawUser) return null;

  return {
    ...rawUser,
    isAdmin: rawUser.roles?.includes("Admin") || false,
  };
}
