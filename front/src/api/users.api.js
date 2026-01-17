import api from "./axios";
import { API_ROUTES } from "../constants/api-routes";

export const getUsers = () => {
  return api.get(API_ROUTES.USERS.GET_ALL);
};

export const getUserById = (id) => {
  return api.get(API_ROUTES.USERS.GET_BY_ID(id));
};

export const createUser = (userData) => {
  return api.post(API_ROUTES.USERS.CREATE, userData);
};

export const updateUserRoles = (id, roles) => {
  return api.put(API_ROUTES.USERS.UPDATE_ROLES(id), {
    roles,
  });
};

export const deleteUser = (id) => {
  return api.delete(API_ROUTES.USERS.DELETE(id));
};
