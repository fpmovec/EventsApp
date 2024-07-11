import { PayloadAction } from "@reduxjs/toolkit";
import { AuthResult } from "./Models/AuthResponse";
import { Refresh } from "./Requests/POST/Auth";


const fetchWithRetry = (
  input: RequestInfo | URL,
  tokens: AuthResult,
  init?: RequestInit,
  tries: number = 2,
  dispath?: (state: WritableDraft<AuthState>, action: PayloadAction<AuthResult>) => void 
) =>
  fetch(input, init)
    .then((response) => {
      if (response.ok) {
        return response.json();
      } else {
        if (response.status !== 401 && response.status.toString()[0] === "5")
          throw new Error4xx();
        return Promise.reject(response);
      }
    })
    .catch((error) => {
      if (error instanceof Error || tries < 1) throw error;
      Refresh(tokens).then((res) => {
        fetchWithRetry(input, res, init, tries - 1);
      });
    });

export default fetchWithRetry;

class Error4xx extends Error {}
