import { AuthUser } from "../Authorization/Auth";
import { AuthResult } from "../DTOs/AuthResponse";

interface AuthState {
  readonly isAuthenticated: boolean;
  readonly user: AuthUser | undefined;
  readonly tokens: AuthResult;
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
};
