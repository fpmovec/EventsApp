import { AuthUser } from "../../Authorization/Auth";
import { AuthResult } from "../../DTOs/AuthResponse";
import { useAppSelector } from "../../Redux/Hooks";

export const GetAccessTokens = (): AuthResult => {
  const result = useAppSelector((state) => state.auth.tokens);
  return result;
};

const baseUrl = "https://localhost:7107";

export const GetCurrentUser = async (
  token: string
): Promise<AuthUser | undefined> => {
  const response = await fetch(`${baseUrl}/api/user/current`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
  });
  return await response.json();
};

export const IsAuthenticated = async (token: string): Promise<boolean> => {
  console.log(token);
  const response = await fetch(`${baseUrl}/api/user/isAuthenticated`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
  });

  return await response.json();
};
