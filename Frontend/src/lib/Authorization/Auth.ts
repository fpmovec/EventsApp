import React from "react";
import { AuthResult } from "../DTOs/AuthResponse";
import { LoginUserDTO, RegisterUserDTO } from "../DTOs/RegisterLogin";

interface AuthContext {
  isAuthenticated: boolean;
  user?: AuthUser;
  signIn: (data: LoginUserDTO) => void;
  register: (data: RegisterUserDTO) => void;
  refresh: (data: AuthResult) => void;
  signOut: () => void;
  mainToken?: string;
  refreshToken?: string;
}

export interface AuthUser {
  id: string;
  email: string;
  name: string;
  phone: string;
}

export const AuthContext = React.createContext<AuthContext>({
  isAuthenticated: false,
  signIn: () => {},
  register: () => {},
  refresh: () => {},
  signOut: () => {},
});

export const useAuth = () => React.useContext(AuthContext);
