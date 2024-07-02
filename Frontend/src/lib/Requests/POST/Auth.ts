import { useDispatch } from "react-redux";
import { AuthResult } from "../../DTOs/AuthResponse";
import { LoginUserDTO, RegisterUserDTO } from "../../DTOs/RegisterLogin";
import { useAppSelector } from "../../Redux/Hooks";
import { refresh } from "../../Redux/Slices";

//const token = GetAccessTokens().mainToken;
const baseUrl = "https://localhost:7107";

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

export const CheckTokenAndRefresh = async <T>(
  request: () => Response
): Promise<T> => {
  let response = request();
  const currentTokens = useAppSelector((state) => state.auth.tokens);
  const dispatch = useDispatch();

  if (response.status === 401) {
    const tokens = await Refresh(currentTokens);
    dispatch(refresh(tokens));

    response = request();
    if (response.status === 401) return await response.json();
  }

  const result = await response.json();

  return result as T;
};
