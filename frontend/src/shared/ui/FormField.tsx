import type { ReactNode } from 'react';

type FormFieldProps = {
  label: string;
  error?: string;
  children: ReactNode;
  className?: string;
};

export function FormField({ label, error, children, className }: FormFieldProps) {
  return (
    <label className={`block text-sm ${className ?? ''}`}>
      <span className="font-medium text-slate-700">{label}</span>
      <div className="mt-1">{children}</div>
      {error && <p className="mt-1 text-xs text-red-600">{error}</p>}
    </label>
  );
}

export const inputClass =
  'w-full rounded-md border border-slate-300 px-3 py-2 text-sm focus:border-brand-600 focus:outline-none focus:ring-1 focus:ring-brand-600';
