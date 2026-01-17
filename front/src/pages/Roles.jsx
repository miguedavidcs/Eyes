import { useEffect, useState } from "react";
import Navbar from "../components/Navbar";
import {
  getRoles,
  createRole,
  updateRole,
  deleteRole,
} from "../api/roles.api";

export default function Roles() {
  const [roles, setRoles] = useState([]);
  const [name, setName] = useState("");
  const [editing, setEditing] = useState(null);

  const loadRoles = async () => {
    const res = await getRoles();
    setRoles(res.data);
  };

  useEffect(() => {
    loadRoles();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!name.trim()) return;

    if (editing) {
      await updateRole(editing.id, {
        id: editing.id,
        name,
      });
    } else {
      await createRole({ name });
    }

    setName("");
    setEditing(null);
    loadRoles();
  };

  const handleEdit = (role) => {
    setEditing(role);
    setName(role.name);
  };

  const handleDelete = async (id) => {
    if (!confirm("¿Eliminar rol?")) return;
    await deleteRole(id);
    loadRoles();
  };

  return (
    <>
      <Navbar />
      <div className="container mt-4">
        <h2>Roles</h2>

        <form onSubmit={handleSubmit} className="mb-3">
          <input
            className="form-control mb-2"
            placeholder="Nombre del rol"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />

          <button className="btn btn-primary">
            {editing ? "Actualizar" : "Crear"}
          </button>

          {editing && (
            <button
              type="button"
              className="btn btn-secondary ms-2"
              onClick={() => {
                setEditing(null);
                setName("");
              }}
            >
              Cancelar
            </button>
          )}
        </form>

        <table className="table table-bordered">
          <thead className="table-dark">
            <tr>
              <th>Rol</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            {roles.map((r) => (
              <tr key={r.id}>
                <td>{r.name}</td>
                <td>
                  <button
                    className="btn btn-sm btn-warning me-2"
                    onClick={() => handleEdit(r)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn btn-sm btn-danger"
                    onClick={() => handleDelete(r.id)}
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
