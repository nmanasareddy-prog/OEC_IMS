import { useDashboardMetrics } from '@/features/dashboard/api/use-dashboard';
import { PageHeader } from '@/shared/ui/PageHeader';

export function DashboardPage() {
  const { data, isLoading, isError } = useDashboardMetrics();

  return (
    <>
      <PageHeader title="Dashboard" description="Inventory KPIs and recent stock activity." />
      {isLoading && <p className="text-slate-500">Loading metrics…</p>}
      {isError && <p className="text-red-600">Failed to load dashboard.</p>}
      {data && (
        <>
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            {[
              { label: 'Total Parts', value: data.totalParts },
              { label: 'Low Stock', value: data.lowStockCount },
              { label: 'Pending Orders', value: data.pendingOrdersCount },
              { label: 'Recent Events', value: data.recentActivity.length },
            ].map((card) => (
              <div key={card.label} className="rounded-lg border border-slate-200 bg-white p-4 shadow-sm">
                <p className="text-sm text-slate-500">{card.label}</p>
                <p className="mt-2 text-2xl font-semibold text-slate-900">{card.value}</p>
              </div>
            ))}
          </div>
          <div className="mt-8 rounded-lg border border-slate-200 bg-white shadow-sm">
            <h3 className="border-b px-4 py-3 text-sm font-semibold text-slate-700">Recent stock activity</h3>
            <ul className="divide-y">
              {data.recentActivity.map((a) => (
                <li key={a.stockMovementId} className="flex justify-between px-4 py-3 text-sm">
                  <span>
                    <span className="font-mono text-xs text-slate-500">{a.partSku}</span> — {a.partName}
                  </span>
                  <span className={a.quantityChange < 0 ? 'text-red-600' : 'text-green-600'}>
                    {a.quantityChange > 0 ? '+' : ''}{a.quantityChange} ({a.movementType})
                  </span>
                </li>
              ))}
              {data.recentActivity.length === 0 && (
                <li className="px-4 py-6 text-center text-slate-500">No activity yet.</li>
              )}
            </ul>
          </div>
        </>
      )}
    </>
  );
}
