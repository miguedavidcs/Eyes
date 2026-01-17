import api from "./axios";
import { API_ROUTES } from "../constants/api-routes";

export const loginRequest = (email, password) => {
  return api.post(API_ROUTES.AUTH.LOGIN, { email, password });
};

export const registerRequest = (data) => {
  return api.post(API_ROUTES.AUTH.REGISTER, data);
};
