import { useMemo, useState } from 'react';
import { createColumnHelper } from '@tanstack/react-table';
import {
  useCancelOrder,
  useCreateOrder,
  useOrder,
  useOrders,
} from '@/features/orders/api/use-orders';
import { CreateOrderForm } from '@/features/orders/components/CreateOrderForm';
import { OrderDetail } from '@/features/orders/components/OrderDetail';
import { useParts } from '@/features/parts/api/use-parts';
import { ConfirmDialog } from '@/shared/ui/ConfirmDialog';
import { DataTable } from '@/shared/ui/DataTable';
import { Modal } from '@/shared/ui/Modal';
import { PageHeader } from '@/shared/ui/PageHeader';
import type { OrderListItem } from '@/shared/types/api';

const columnHelper = createColumnHelper<OrderListItem>();

export function OrdersPage() {
  const [page, setPage] = useState(1);
  const [detailId, setDetailId] = useState<number | null>(null);
  const [cancelId, setCancelId] = useState<number | null>(null);

  const orders = useOrders(page);
  const parts = useParts({ page: 1, pageSize: 100 });
  const orderDetail = useOrder(detailId ?? undefined);
  const createOrder = useCreateOrder();
  const cancelOrder = useCancelOrder();

  const columns = useMemo(
    () => [
      columnHelper.accessor('orderNumber', {
        header: 'Order #',
        cell: (c) => <span className="font-mono text-xs">{c.getValue()}</span>,
      }),
      columnHelper.accessor('status', { header: 'Status' }),
      columnHelper.accessor('totalAmount', {
        header: 'Total',
        cell: (c) => `$${c.getValue().toFixed(2)}`,
      }),
      columnHelper.accessor('lineCount', { header: 'Lines' }),
      columnHelper.accessor('createdAt', {
        header: 'Created',
        cell: (c) => new Date(c.getValue()).toLocaleString(),
      }),
      columnHelper.display({
        id: 'actions',
        header: '',
        cell: ({ row }) => (
          <div className="flex gap-3">
            <button
              type="button"
              className="text-sm text-brand-600 hover:underline"
              onClick={() => setDetailId(row.original.orderId)}
            >
              View
            </button>
            {row.original.status === 'Pending' && (
              <button
                type="button"
                className="text-sm text-red-600 hover:underline"
                onClick={() => setCancelId(row.original.orderId)}
              >
                Cancel
              </button>
            )}
          </div>
        ),
      }),
    ],
    [],
  );

  return (
    <>
      <PageHeader
        title="Orders"
        description="Create orders with inventory deduction; view line items; cancel pending orders."
      />

      <div className="mb-8 rounded-lg border border-slate-200 bg-white p-5 shadow-sm">
        <h3 className="text-sm font-semibold text-slate-900">New order</h3>
        {parts.data && (
          <div className="mt-4">
            <CreateOrderForm
              parts={parts.data.items}
              onSubmit={(values) => createOrder.mutate(values)}
              isPending={createOrder.isPending}
            />
          </div>
        )}
      </div>

      {orders.isLoading && <p className="text-slate-500">Loading orders…</p>}
      {orders.isError && <p className="text-red-600">Failed to load orders.</p>}

      {orders.data && (
        <div className="rounded-lg border border-slate-200 bg-white shadow-sm">
          <DataTable data={orders.data.items} columns={columns} />
          <div className="flex items-center justify-between border-t px-4 py-3 text-sm text-slate-600">
            <span>
              Page {orders.data.page} of {orders.data.totalPages}
            </span>
            <div className="flex gap-2">
              <button
                type="button"
                disabled={page <= 1}
                className="rounded border px-2 py-1 disabled:opacity-40"
                onClick={() => setPage((p) => p - 1)}
              >
                Prev
              </button>
              <button
                type="button"
                disabled={page >= orders.data.totalPages}
                className="rounded border px-2 py-1 disabled:opacity-40"
                onClick={() => setPage((p) => p + 1)}
              >
                Next
              </button>
            </div>
          </div>
        </div>
      )}

      <Modal
        open={detailId !== null}
        title="Order details"
        onClose={() => setDetailId(null)}
      >
        {orderDetail.isLoading && <p className="text-slate-500">Loading…</p>}
        {orderDetail.data && <OrderDetail order={orderDetail.data} />}
      </Modal>

      <ConfirmDialog
        open={cancelId !== null}
        title="Cancel order"
        message="Cancel this pending order and restore inventory?"
        confirmLabel="Cancel order"
        loading={cancelOrder.isPending}
        onCancel={() => setCancelId(null)}
        onConfirm={() => {
          if (cancelId) {
            cancelOrder.mutate(cancelId, { onSuccess: () => setCancelId(null) });
          }
        }}
      />
    </>
  );
}
