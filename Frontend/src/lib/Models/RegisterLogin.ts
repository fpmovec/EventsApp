interface RegisterUserDTO {
  name: string;
  email: string;
  password: string;
  phone: string;
}

interface LoginUserDTO {
  email: string;
  password: string;
}

export type { RegisterUserDTO, LoginUserDTO };
