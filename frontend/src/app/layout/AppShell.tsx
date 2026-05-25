import { NavLink, Outlet } from 'react-router-dom';
import { useAuth } from '@/features/auth/context/AuthContext';
import { getRolesFromToken } from '@/features/auth/lib/jwt';

const navItems = [
  { to: '/dashboard', label: 'Dashboard' },
  { to: '/parts', label: 'Parts' },
  { to: '/vehicles', label: 'Vehicles' },
  { to: '/orders', label: 'Orders' },
] as const;

export function AppShell() {
  const { token, logout } = useAuth();
  const roles = getRolesFromToken(token);

  return (
    <div className="flex min-h-screen">
      <aside className="flex w-64 shrink-0 flex-col border-r border-slate-200 bg-brand-900 text-white">
        <div className="border-b border-white/10 px-6 py-5">
          <p className="text-xs uppercase tracking-wider text-slate-300">OEC</p>
          <h1 className="text-lg font-semibold">Inventory MS</h1>
          {roles.length > 0 && (
            <p className="mt-1 text-xs text-slate-400">{roles.join(' · ')}</p>
          )}
        </div>
        <nav className="flex flex-col gap-1 p-4">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `rounded-md px-3 py-2 text-sm font-medium transition-colors ${
                  isActive
                    ? 'bg-brand-600 text-white'
                    : 'text-slate-200 hover:bg-white/10'
                }`
              }
            >
              {item.label}
            </NavLink>
          ))}
        </nav>
        <button
          type="button"
          className="m-4 mt-auto rounded-md border border-white/20 px-3 py-2 text-sm text-slate-200 hover:bg-white/10"
          onClick={logout}
        >
          Sign out
        </button>
      </aside>
      <main className="flex-1 overflow-auto p-8">
        <Outlet />
      </main>
    </div>
  );
}
