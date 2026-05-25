import { useState } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '@/features/auth/context/AuthContext';
import { useLogin } from '@/features/auth/api/use-login';

export function LoginPage() {
  const { isAuthenticated } = useAuth();
  const login = useLogin();
  const [username, setUsername] = useState('admin');
  const [password, setPassword] = useState('Admin123!');

  if (isAuthenticated) {
    return <Navigate to="/dashboard" replace />;
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100">
      <div className="w-full max-w-md rounded-xl border border-slate-200 bg-white p-8 shadow-sm">
        <h1 className="text-xl font-semibold text-brand-900">OEC Inventory MS</h1>
        <p className="mt-2 text-sm text-slate-600">
          Demo: admin / Admin123! or clerk / Clerk123!
        </p>
        <label className="mt-6 block text-sm font-medium text-slate-700">
          Username
          <input
            className="mt-1 w-full rounded-md border border-slate-300 px-3 py-2"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </label>
        <label className="mt-4 block text-sm font-medium text-slate-700">
          Password
          <input
            type="password"
            className="mt-1 w-full rounded-md border border-slate-300 px-3 py-2"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </label>
        {login.isError && (
          <p className="mt-3 text-sm text-red-600">Invalid credentials. Please try again.</p>
        )}
        <button
          type="button"
          disabled={login.isPending}
          className="mt-6 w-full rounded-md bg-brand-600 px-4 py-2 text-sm font-medium text-white hover:bg-brand-900 disabled:opacity-60"
          onClick={() => login.mutate({ username, password })}
        >
          {login.isPending ? 'Signing in…' : 'Sign in'}
        </button>
      </div>
    </div>
  );
}
