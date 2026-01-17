import {jwtDecode} from "jwt-decode"

export function decodeToken(token){
    if(!token) return null;
    try{
        return jwtDecode(token);
    }catch(error){
        console.error("Error de decodificacion del Token:", error);
        return null
    }
}