import { useEffect, useState } from "react";
import Navbar from "../components/Navbar";
import { getUsers, updateUserRoles, deleteUser } from "../api/users.api";
import { getRoles } from "../api/roles.api";

export default function AdminDashboard() {
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const loadData = async () => {
    try {
      setLoading(true);
      const [usersRes, rolesRes] = await Promise.all([
        getUsers(),
        getRoles(),
      ]);

      // usersRes.data debe traer: { id, email, fullName, roles: [] }
      setUsers(usersRes.data);
      console.log(usersRes.data);
      // rolesRes.data debe traer: { id, name }
      setRoles(rolesRes.data);
      console.log(rolesRes.data);
    } catch (e) {
      setError("Error--- cargando datos", e.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadData();
  }, []);

  // Maneja el cambio local (draft) de roles por usuario
  const handleRolesChange = (userId, selectedRoles) => {
    setUsers((prev) =>
      prev.map((u) =>
        u.id === userId ? { ...u, _rolesDraft: selectedRoles } : u
      )
    );
  };

  // Guarda los roles del usuario
  const handleSaveRoles = async (u) => {
    const rolesToSave = u._rolesDraft ?? u.roles ?? [];
    await updateUserRoles(u.id, rolesToSave);
    await loadData();
  };

  const handleDelete = async (id) => {
    if (!confirm("¿Eliminar usuario?")) return;
    await deleteUser(id);
    await loadData();
  };

  if (loading) {
    return (
      <>
        <Navbar />
        <p className="m-4">Cargando...</p>
      </>
    );
  }

  if (error) {
    return (
      <>
        <Navbar />
        <div className="alert alert-danger m-4">{error}</div>
      </>
    );
  }

  return (
    <>
      <Navbar />
      <div className="container mt-4">
        <h2>Administración de Usuarios</h2>

        <table className="table table-bordered mt-3">
          <thead className="table-dark">
            <tr>
              <th>Email</th>
              <th>Nombre</th>
              <th>Roles</th>
              <th>Acciones</th>
            </tr>
          </thead>

          <tbody>
            {users.map((u) => (
              <tr key={u.id}>
                <td>{u.email}</td>
                <td>{u.fullName}</td>

                <td>
                  <select
                    multiple
                    className="form-select"
                    value={u._rolesDraft ?? u.roles ?? []}
                    onChange={(e) =>
                      handleRolesChange(
                        u.id,
                        Array.from(e.target.selectedOptions).map(
                          (o) => o.value
                        )
                      )
                    }
                  >
                    {roles.map((r) => (
                      <option key={r.id} value={r.name}>
                        {r.name}
                      </option>
                    ))}
                  </select>
                </td>

                <td>
                  <button
                    className="btn btn-sm btn-primary me-2"
                    onClick={() => handleSaveRoles(u)}
                  >
                    Guardar roles
                  </button>

                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(u.id)}
                  >
                    Eliminar
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </>
  );
}
