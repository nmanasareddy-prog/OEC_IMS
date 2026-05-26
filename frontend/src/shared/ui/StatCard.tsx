import type { ReactNode } from 'react';

type StatCardProps = {
  label: string;
  value: string | number;
  hint?: string;
  icon?: ReactNode;
  tone?: 'default' | 'warning' | 'success' | 'info';
};

const toneStyles = {
  default: 'from-slate-50 to-white border-slate-200/80',
  warning: 'from-amber-50 to-white border-amber-200/80',
  success: 'from-emerald-50 to-white border-emerald-200/80',
  info: 'from-blue-50 to-white border-blue-200/80',
};

const iconStyles = {
  default: 'bg-slate-100 text-slate-600',
  warning: 'bg-amber-100 text-amber-700',
  success: 'bg-emerald-100 text-emerald-700',
  info: 'bg-blue-100 text-blue-700',
};

export function StatCard({ label, value, hint, icon, tone = 'default' }: StatCardProps) {
  return (
    <div
      className={`rounded-xl border bg-gradient-to-br p-5 shadow-sm ${toneStyles[tone]}`}
    >
      <div className="flex items-start justify-between gap-3">
        <div>
          <p className="text-sm font-medium text-slate-500">{label}</p>
          <p className="mt-2 text-3xl font-bold tracking-tight text-slate-900">{value}</p>
          {hint && <p className="mt-1 text-xs text-slate-500">{hint}</p>}
        </div>
        {icon && (
          <div className={`flex h-11 w-11 shrink-0 items-center justify-center rounded-xl ${iconStyles[tone]}`}>
            {icon}
          </div>
        )}
      </div>
    </div>
  );
}
