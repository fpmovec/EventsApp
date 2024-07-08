import { AuthUser } from "../Authorization/Auth";
import { AuthResult } from "../Models/AuthResponse";

interface AuthState {
  readonly isAuthenticated: boolean;
  readonly user: AuthUser | undefined;
  readonly tokens: AuthResult;
  readonly categories: string[];
}

export interface AppState {
  auth: AuthState;
}

export const initialState: AuthState = {
  isAuthenticated: false,
  user: undefined,
  tokens: {
    mainToken: "",
    refreshToken: "",
  },
  categories: []
};
