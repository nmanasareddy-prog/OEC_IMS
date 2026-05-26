import { Navigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { useAuth } from '@/features/auth/context/AuthContext';
import { useLogin } from '@/features/auth/api/use-login';
import { loginSchema, type LoginFormValues } from '@/features/auth/schemas/login.schema';
import { formResolver } from '@/shared/lib/form-resolver';
import { FormField, inputClass } from '@/shared/ui/FormField';
import { Button } from '@/shared/ui/Button';

export function LoginPage() {
  const { isAuthenticated } = useAuth();
  const login = useLogin();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormValues>({
    resolver: formResolver(loginSchema),
    defaultValues: { username: 'admin', password: 'Admin123!' },
  });

  if (isAuthenticated) {
    return <Navigate to="/dashboard" replace />;
  }

  return (
    <div className="flex min-h-screen">
      <div className="hidden flex-1 flex-col justify-between bg-gradient-to-br from-brand-950 via-brand-900 to-brand-700 p-12 text-white lg:flex">
        <div>
          <p className="text-sm font-semibold uppercase tracking-widest text-brand-500">OEC Automotive</p>
          <h1 className="mt-6 max-w-md text-4xl font-bold leading-tight">
            Inventory built for dealer-grade parts operations
          </h1>
          <p className="mt-4 max-w-sm text-slate-300">
            MediatR · FluentValidation · EF Core on the API. React 19 · TanStack Query · Tailwind v4 on the UI.
          </p>
        </div>
        <ul className="space-y-2 text-sm text-slate-400">
          <li>Real-time stock & low-stock alerts</li>
          <li>Vehicle fitment & compatibility</li>
          <li>Transactional orders with audit trail</li>
        </ul>
      </div>

      <div className="flex flex-1 items-center justify-center bg-slate-100 p-6">
        <div className="w-full max-w-md rounded-2xl border border-slate-200/80 bg-white p-8 shadow-xl shadow-slate-300/40">
          <h2 className="text-xl font-bold text-slate-900">Sign in</h2>
          <p className="mt-1 text-sm text-slate-500">
            Demo: <span className="font-mono text-xs">admin</span> / Admin123! or clerk / Clerk123!
          </p>

          <form
            className="mt-8 space-y-5"
            onSubmit={handleSubmit((values) => login.mutate(values))}
          >
            <FormField label="Username" error={errors.username?.message}>
              <input className={inputClass} autoComplete="username" {...register('username')} />
            </FormField>
            <FormField label="Password" error={errors.password?.message}>
              <input
                type="password"
                className={inputClass}
                autoComplete="current-password"
                {...register('password')}
              />
            </FormField>
            {login.isError && (
              <p className="rounded-lg bg-red-50 px-3 py-2 text-sm text-red-700">
                Invalid credentials. Please try again.
              </p>
            )}
            <Button
              type="submit"
              className="w-full"
              disabled={login.isPending}
            >
              {login.isPending ? 'Signing in…' : 'Sign in'}
            </Button>
          </form>
        </div>
      </div>
    </div>
  );
}
