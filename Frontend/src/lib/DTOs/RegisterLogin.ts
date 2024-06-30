interface RegisterUserDTO {
  name: string;
  email: string;
  password: string;
}

interface LoginUserDTO {
  email: string;
  password: string;
}

export type { RegisterUserDTO, LoginUserDTO };
