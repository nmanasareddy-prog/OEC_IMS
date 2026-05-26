import { useMemo } from 'react';
import { createColumnHelper } from '@tanstack/react-table';
import { Link } from 'react-router-dom';
import { useDashboardMetrics } from '@/features/dashboard/api/use-dashboard';
import { PageHeader } from '@/shared/ui/PageHeader';
import { StatCard } from '@/shared/ui/StatCard';
import { Card, CardHeader } from '@/shared/ui/Card';
import { DataTable } from '@/shared/ui/DataTable';
import { LoadingSpinner } from '@/shared/ui/LoadingSpinner';
import { EmptyState } from '@/shared/ui/EmptyState';
import { Button } from '@/shared/ui/Button';
import type { DashboardMetrics } from '@/shared/types/api';

type ActivityRow = DashboardMetrics['recentActivity'][number];
const columnHelper = createColumnHelper<ActivityRow>();

export function DashboardPage() {
  const { data, isLoading, isError } = useDashboardMetrics();

  const activityColumns = useMemo(
    () => [
      columnHelper.accessor('partSku', {
        header: 'SKU',
        cell: (c) => <span className="font-mono text-xs text-slate-600">{c.getValue()}</span>,
      }),
      columnHelper.accessor('partName', { header: 'Part' }),
      columnHelper.accessor('movementType', { header: 'Type' }),
      columnHelper.accessor('quantityChange', {
        header: 'Change',
        cell: (c) => {
          const v = c.getValue();
          return (
            <span className={v < 0 ? 'font-semibold text-red-600' : 'font-semibold text-emerald-600'}>
              {v > 0 ? '+' : ''}
              {v}
            </span>
          );
        },
      }),
      columnHelper.accessor('occurredAt', {
        header: 'When',
        cell: (c) => new Date(c.getValue()).toLocaleString(),
      }),
    ],
    [],
  );

  return (
    <>
      <PageHeader
        title="Operations dashboard"
        description="KPIs powered by TanStack Query; stock movements from EF Core audit data."
        actions={
          <Link to="/parts">
            <Button variant="secondary">Manage parts</Button>
          </Link>
        }
      />

      {isLoading && <LoadingSpinner label="Loading dashboard metrics…" />}
      {isError && (
        <p className="rounded-lg bg-red-50 px-4 py-3 text-sm text-red-700">
          Failed to load dashboard. Check API connection.
        </p>
      )}

      {data && (
        <>
          <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-4">
            <StatCard
              label="Total parts"
              value={data.totalParts}
              tone="info"
              icon={<span className="text-lg font-bold">P</span>}
            />
            <StatCard
              label="Low stock"
              value={data.lowStockCount}
              tone="warning"
              hint="Below reorder level"
              icon={<span className="text-lg font-bold">!</span>}
            />
            <StatCard
              label="Pending orders"
              value={data.pendingOrdersCount}
              tone="default"
              icon={<span className="text-lg font-bold">O</span>}
            />
            <StatCard
              label="Recent movements"
              value={data.recentActivity.length}
              tone="success"
              icon={<span className="text-lg font-bold">↕</span>}
            />
          </div>

          <Card className="mt-8" padding={false}>
            <CardHeader title="Recent stock activity" description="Last movements from inventory transactions" />
            {data.recentActivity.length === 0 ? (
              <EmptyState
                title="No stock activity yet"
                description="Adjust stock or place an order to see movements here."
              />
            ) : (
              <DataTable data={data.recentActivity} columns={activityColumns} />
            )}
          </Card>
        </>
      )}
    </>
  );
}
