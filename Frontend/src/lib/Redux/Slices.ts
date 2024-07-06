import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { initialState } from "./StoreModels";
import { AuthUser } from "../Authorization/Auth";
import { AuthResult } from "../Models/AuthResponse";

type SignData = {
  user: AuthUser;
  tokens: AuthResult;
};

const authSlice = createSlice({
  name: "auth",
  initialState: initialState,
  reducers: {
    signOut: (state) => {
      state.isAuthenticated = false;
      state.tokens.mainToken = "";
      state.tokens.refreshToken = "";
    },

    signIn: (state, action: PayloadAction<SignData>) => {
      state.user = action.payload.user;
      state.tokens = action.payload.tokens;
      state.isAuthenticated = true;
    },

    refresh: (state, action: PayloadAction<AuthResult>) => {
      state.tokens = action.payload;
      state.isAuthenticated = true;
    },

    reg: (state, action: PayloadAction<SignData>) => {
      state.user = action.payload.user;
      state.tokens = action.payload.tokens;
      state.isAuthenticated = true;
    },

    setTokens: (state, action: PayloadAction<AuthResult>) => {
      state.tokens = action.payload;
    },
  },
});

export const { signOut, signIn, refresh, reg, setTokens } = authSlice.actions;

export default authSlice.reducer;
