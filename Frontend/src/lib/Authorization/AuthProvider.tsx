import React, { useState } from "react";
import { AuthContext, AuthUser } from "./Auth";
import { Login, Logout, Refresh, Register } from "../Requests/POST/Auth";
import { AuthResult } from "../DTOs/AuthResponse";
import { GetCurrentUser, IsAuthenticated } from "../Requests/GET/Auth";
import { LoginUserDTO, RegisterUserDTO } from "../DTOs/RegisterLogin";

type Props = {
  children?: React.ReactNode;
};

const AuthProvider: React.FC<Props> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [user, setUser] = useState<AuthUser | undefined>(undefined);
  const [authTokens, setAuthTokens] = useState<AuthResult | null>(null);

  React.useEffect(() => {
    const initAuth = async () => {
      const isAuthenticated = await IsAuthenticated();

      setIsAuthenticated(isAuthenticated);
      if (isAuthenticated) {
        const currentUser = await GetCurrentUser();
        setUser(currentUser);
      }
    };
    initAuth();
  }, []);

  const sign = async (data: LoginUserDTO) => {
    const result = await Login(data);
    console.log(result);
    setAuthTokens(result);
  };

  const signO = async () => {
    await Logout();
    setAuthTokens({ mainToken: "", refreshToken: "" });
  };

  const reg = async (data: RegisterUserDTO) => {
    const result = await Register(data);
    setAuthTokens(result);
  };

  const refr = async (data: AuthResult) => {
    const result = await Refresh(data);
    setAuthTokens(result);
  };

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        signIn: sign,
        signOut: signO,
        register: reg,
        refresh: refr,
        user,
        mainToken: authTokens?.mainToken,
        refreshToken: authTokens?.refreshToken,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
