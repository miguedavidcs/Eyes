import api from "./axios";
import { API_ROUTES } from "../constants/api-routes";

export const getRoles = () =>
  api.get(API_ROUTES.ROLES.GET_ALL);

export const getRoleById = (id) =>
  api.get(API_ROUTES.ROLES.GET_BY_ID(id));

export const createRole = (data) =>
  api.post(API_ROUTES.ROLES.CREATE, data);

export const updateRole = (id, data) =>
  api.put(API_ROUTES.ROLES.UPDATE(id), data);

export const deleteRole = (id) =>
  api.delete(API_ROUTES.ROLES.DELETE(id));
