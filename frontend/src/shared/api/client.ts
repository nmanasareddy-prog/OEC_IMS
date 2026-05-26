const PROD_API_BASE = import.meta.env.VITE_API_URL?.replace(/\/$/, '');

if (import.meta.env.PROD && !PROD_API_BASE) {
  throw new Error(
    'Production build requires VITE_API_URL to be set to your permanent backend URL.',
  );
}

const API_BASE = import.meta.env.PROD ? PROD_API_BASE : '';

export type ApiError = {
  title?: string;
  detail?: string;
  status?: number;
  errors?: Record<string, string[]>;
};

export async function apiFetch<T>(
  path: string,
  init?: RequestInit,
): Promise<T> {
  const token = sessionStorage.getItem('oec-ims-token');
  const headers = new Headers(init?.headers);
  headers.set('Content-Type', 'application/json');
  if (token) {
    headers.set('Authorization', `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE}${path}`, { ...init, headers });

  if (!response.ok) {
    const problem = (await response.json().catch(() => ({}))) as ApiError;
    throw Object.assign(new Error(problem.detail ?? problem.title ?? 'Request failed'), {
      problem,
      status: response.status,
    });
  }

  if (response.status === 204) {
    return undefined as T;
  }

  return (await response.json()) as T;
}
