export interface AuthUser {
  email: string;
  fullName: string;
}

export interface AuthData {
  token: string;
  email: string;
  fullName: string;
}

export interface AuthApiResponse {
  success: boolean;
  message: string;
  data: AuthData;
  meta: null;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  fullName: string;
  email: string;
  password: string;
}
