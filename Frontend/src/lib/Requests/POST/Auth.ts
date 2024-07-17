import { baseUrl } from "../../Constants";
import { AuthResult } from "../../Models/AuthResponse";
import { LoginUserDTO, RegisterUserDTO } from "../../Models/RegisterLogin";

export const Login = async (data: LoginUserDTO): Promise<AuthResult> => {
  let result: AuthResult = { mainToken: "", refreshToken: "" };

  const response = await fetch(`${baseUrl}/api/auth/login`, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });

  result = await response.json();

  return result;
};

export const Register = async (data: RegisterUserDTO): Promise<AuthResult> => {
  let result: AuthResult = { mainToken: "", refreshToken: "" };

  const response = await fetch(`${baseUrl}/api/auth/register`, {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });

  result = await response.json();
  return result;
};

export const Refresh = async (data: AuthResult): Promise<AuthResult> => {
  let result: AuthResult = { mainToken: "", refreshToken: "" };

  const response = await fetch(`${baseUrl}/api/auth/refresh`, {
    method: "POST",
    headers: {
      Authorization: "Bearer " + `${data.mainToken}`,
      "Content-Type": "application/json",
    },
    credentials: "omit",
    body: JSON.stringify(data),
  });

  result = await response.json();
  return result;
};

export const Logout = async (token: string): Promise<void> => {
  await fetch(`${baseUrl}/api/auth/logout`, {
    method: "DELETE",
    headers: {
      Authorization: "Bearer " + `${token}`,
      "Content-Type": "application/json",
    },
    credentials: "omit",
  });
};
