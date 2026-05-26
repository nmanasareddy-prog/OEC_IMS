import { NavLink, Outlet } from 'react-router-dom';
import { useAuth } from '@/features/auth/context/AuthContext';
import { getRolesFromToken } from '@/features/auth/lib/jwt';

const navItems = [
  { to: '/dashboard', label: 'Dashboard', icon: 'M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6' },
  { to: '/parts', label: 'Parts', icon: 'M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4' },
  { to: '/vehicles', label: 'Vehicles', icon: 'M8 17h.01M12 17h.01M16 17h.01M5 11h14l-1-4H6l-1 4zM5 11l-1.5 6h15L17 11' },
  { to: '/orders', label: 'Orders', icon: 'M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2' },
] as const;

function NavIcon({ d }: { d: string }) {
  return (
    <svg className="h-5 w-5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor" strokeWidth={1.75}>
      <path strokeLinecap="round" strokeLinejoin="round" d={d} />
    </svg>
  );
}

export function AppShell() {
  const { token, logout } = useAuth();
  const roles = getRolesFromToken(token);

  return (
    <div className="flex min-h-screen bg-slate-100">
      <aside className="flex w-72 shrink-0 flex-col bg-gradient-to-b from-brand-950 via-brand-900 to-brand-800 text-white shadow-xl">
        <div className="border-b border-white/10 px-6 py-6">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-brand-600 font-bold shadow-lg shadow-brand-600/30">
              OEC
            </div>
            <div>
              <h1 className="text-base font-bold tracking-tight">Inventory MS</h1>
              <p className="text-xs text-slate-400">Parts & fleet catalog</p>
            </div>
          </div>
        </div>

        <nav className="flex flex-1 flex-col gap-1 p-4">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                `flex items-center gap-3 rounded-lg px-3 py-2.5 text-sm font-medium transition ${
                  isActive
                    ? 'bg-brand-600 text-white shadow-md shadow-brand-600/30'
                    : 'text-slate-300 hover:bg-white/10 hover:text-white'
                }`
              }
            >
              <NavIcon d={item.icon} />
              {item.label}
            </NavLink>
          ))}
        </nav>

        <div className="border-t border-white/10 p-4">
          {roles.length > 0 && (
            <p className="mb-3 truncate text-xs text-slate-400">
              Signed in as <span className="font-medium text-slate-200">{roles.join(', ')}</span>
            </p>
          )}
          <button
            type="button"
            className="w-full rounded-lg border border-white/15 px-3 py-2 text-sm text-slate-200 transition hover:bg-white/10"
            onClick={logout}
          >
            Sign out
          </button>
        </div>
      </aside>

      <main className="page-gradient flex-1 overflow-auto">
        <div className="mx-auto max-w-7xl px-6 py-8 lg:px-10">
          <Outlet />
        </div>
      </main>
    </div>
  );
}
