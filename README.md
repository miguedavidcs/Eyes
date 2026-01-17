# Eyes
Software de Utlidad Masiva - Empresarial
# Enterprice

Sistema empresarial modular con autenticación JWT y autorización basada en **roles y permisos**, diseñado para crecer por fases sin perder control ni seguridad.

Este proyecto no es un demo: es una **base sólida** pensada para productos reales, con énfasis en trazabilidad, control de acceso y mantenibilidad.

---

## 🧠 Visión del proyecto

Enterprice nace como una plataforma backend-first donde:

* El **backend decide** (seguridad, permisos, reglas).
* El **frontend interpreta**, no inventa permisos.
* La arquitectura permite crecer sin reescribir lo crítico.

La idea central es simple y poderosa:

> *Un usuario puede tener múltiples roles, y los roles definen permisos explícitos.*

Nada de `if (isAdmin)` repartidos por todo el sistema.

---

## 🚀 Estado actual – Fase 1 (COMPLETADA)

### ✅ Backend (ASP.NET Core)

* Autenticación con **JWT**
* Autorización por **políticas de permisos**
* Roles y permisos persistidos en base de datos
* Relación:

  * User ↔ UserRole ↔ Role
  * Role ↔ RolePermission ↔ Permission
* Seed automático:

  * Roles base (Admin, etc.)
  * Permisos del sistema
  * Asignación total de permisos al rol Admin

### ✅ Seguridad

* Tokens JWT incluyen:

  * Id de usuario
  * Email
  * Roles
  * Permisos
* Policies del tipo:

  * `DASHBOARD_VIEW`
  * `USERS_VIEW`
  * `USERS_CREATE`
  * `ROLES_VIEW`
  * etc.

Un `403 Forbidden` **es correcto**, no un error.

### ✅ Frontend (React)

* Login funcional
* Protección de rutas (PrivateRoute / RequireAdmin)
* Dashboard Admin operativo
* Consumo de API autenticado

> El frontend ya refleja correctamente errores 401/403 según permisos.

---

## 🧱 Arquitectura (resumen)

### Backend

* ASP.NET Core
* Entity Framework Core
* SQL Server
* JWT Bearer Authentication
* AutoMapper

Estructura lógica:

* Controllers → validan acceso
* Services → reglas de negocio
* Repositories → acceso a datos
* Security → JWT, permisos, contexto de usuario

### Frontend

* React + Vite
* Axios
* React Router

---

## 🔐 Modelo de permisos

Ejemplo real de permisos:

* `DASHBOARD_VIEW`
* `USERS_VIEW`
* `USERS_CREATE`
* `USERS_UPDATE`
* `USERS_DELETE`
* `ROLES_VIEW`
* `ROLES_CREATE`
* `ROLES_UPDATE`

Los permisos se asignan **a roles**, no directamente a usuarios.

---

## 📌 Fase 2 (PRÓXIMA)

Objetivos:

1. Limpiar y organizar el frontend
2. Controlar UI por permisos:

   * Mostrar/ocultar botones
   * Proteger rutas por permiso
3. Layout Admin (sidebar + contenido)
4. Mejorar UX sin romper la seguridad

Regla clave:

> El frontend **no decide permisos**, solo los interpreta desde el JWT.

---

## 🛠️ Instalación (resumen)

### Backend

```bash
dotnet restore
dotnet build
dotnet run
```

Configurar en `appsettings.json`:

* ConnectionStrings
* Jwt (Key, Issuer, Audience, ExpirationHours)

### Frontend

```bash
npm install
npm run dev
```

---

## 🧪 Filosofía del proyecto

* Primero estabilidad, luego estética
* Seguridad antes que comodidad
* Fases cerradas, no código a medio camino

Este proyecto está pensado para **escala real**, no solo para aprender.

---

## 👤 Autor

Miguel David Castaño Salgado
Ingeniero de Sistemas – Auditoría, Seguridad y Desarrollo

---

> *Fase 1 cerrada. La base está puesta. Ahora empieza lo interesante.*
