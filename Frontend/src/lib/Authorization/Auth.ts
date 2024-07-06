import React from "react";
import { AuthResult } from "../Models/AuthResponse";
import { LoginUserDTO, RegisterUserDTO } from "../Models/RegisterLogin";

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
