import { useMutation } from '@tanstack/react-query';
import { apiFetch } from '@/shared/api/client';
import type { LoginResponse } from '@/shared/types/api';
import { useAuth } from '@/features/auth/context/AuthContext';

type LoginInput = { username: string; password: string };

export function useLogin() {
  const { login } = useAuth();

  return useMutation({
    mutationFn: (input: LoginInput) =>
      apiFetch<LoginResponse>('/api/v1/auth/login', {
        method: 'POST',
        body: JSON.stringify(input),
      }),
    onSuccess: (data) => login(data.token),
  });
}
