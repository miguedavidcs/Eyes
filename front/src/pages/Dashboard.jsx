import { useAuth } from "../auth/AuthContext";
import Navbar from "../components/Navbar";

export default function Dashboard() {
  const { user, loading } = useAuth();

  if (loading) return null;

  return (
    <>
      <Navbar />
      <div className="container mt-4">
        <h2>Dashboard</h2>

        <p>
          Bienvenido <strong>{user?.email}</strong>
          <br />
          Rol: <strong>{user?.roles?.join(", ")}</strong>
        </p>

        <hr />

        <p>
          Este es tu panel principal.  
          Aquí podrás ver información general según tu rol.
        </p>
      </div>
    </>
  );
}
