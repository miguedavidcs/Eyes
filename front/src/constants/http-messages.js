export const HTTP_MESSAGES = {
   BAD_REQUEST: 400,
   UNAUTHORIZED: 401,
   FORBIDDEN: 403,
   NOT_FOUND: 404,
   INTERNAL_SERVER_ERROR: 500,
   VALIDATION_ERROR: 422,
   CONFLICT: 409,
   TEMPORARY_REDIRECT: 307,
   METHOD_NOT_ALLOWED: 405,
};
export const ERROR_MESSAGES = {
  [HTTP_MESSAGES.BAD_REQUEST]: "La solicitud enviada al servidor es incorrecta.",
  [HTTP_MESSAGES.UNAUTHORIZED]: "No tienes autorización para acceder a este recurso.",
  [HTTP_MESSAGES.FORBIDDEN]: "No tienes permisos para realizar esta acción.",
  [HTTP_MESSAGES.NOT_FOUND]: "El recurso solicitado no fue encontrado en el servidor.",
  [HTTP_MESSAGES.INTERNAL_SERVER_ERROR]: "Ocurrió un error en el servidor. Por favor, intenta más tarde.",
  [HTTP_MESSAGES.VALIDATION_ERROR]: "Los datos proporcionados no cumplen con los requisitos de validación.",
  [HTTP_MESSAGES.CONFLICT]: "Existe un conflicto con el estado actual del recurso.",
  [HTTP_MESSAGES.TEMPORARY_REDIRECT]: "El recurso ha sido temporalmente redirigido a otra ubicación.",
  [HTTP_MESSAGES.METHOD_NOT_ALLOWED]: "El método HTTP utilizado no está permitido para este recurso.",
  DEFAULT: "Ocurrió un error inesperado. Por favor, intenta nuevamente.",
};