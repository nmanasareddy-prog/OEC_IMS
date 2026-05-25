import { useMemo, useState } from 'react';
import { createColumnHelper } from '@tanstack/react-table';
import {
  useAdjustStock,
  useCategories,
  useCreatePart,
  useDeletePart,
  usePart,
  useParts,
  useUpdatePart,
} from '@/features/parts/api/use-parts';
import { AdjustStockForm } from '@/features/parts/components/AdjustStockForm';
import { PartForm } from '@/features/parts/components/PartForm';
import { hasRole } from '@/features/auth/lib/jwt';
import { useAuth } from '@/features/auth/context/AuthContext';
import { ConfirmDialog } from '@/shared/ui/ConfirmDialog';
import { DataTable } from '@/shared/ui/DataTable';
import { Modal } from '@/shared/ui/Modal';
import { PageHeader } from '@/shared/ui/PageHeader';
import type { PartListItem } from '@/shared/types/api';

const columnHelper = createColumnHelper<PartListItem>();

export function PartsPage() {
  const { token } = useAuth();
  const isAdmin = hasRole(token, 'Admin');

  const [search, setSearch] = useState('');
  const [categoryId, setCategoryId] = useState<number | undefined>();
  const [lowStockOnly, setLowStockOnly] = useState(false);
  const [page, setPage] = useState(1);

  const [modal, setModal] = useState<'create' | 'edit' | 'adjust' | null>(null);
  const [selectedId, setSelectedId] = useState<number | null>(null);
  const [deleteId, setDeleteId] = useState<number | null>(null);

  const { data, isLoading, isError } = useParts({ page, search, categoryId, lowStockOnly });
  const categories = useCategories();
  const selectedPart = usePart(selectedId ?? undefined);

  const createPart = useCreatePart();
  const updatePart = useUpdatePart();
  const deletePart = useDeletePart();
  const adjustStock = useAdjustStock();

  const columns = useMemo(
    () => [
      columnHelper.accessor('sku', { header: 'SKU', cell: (c) => <span className="font-mono text-xs">{c.getValue()}</span> }),
      columnHelper.accessor('name', { header: 'Name' }),
      columnHelper.accessor('categoryName', { header: 'Category' }),
      columnHelper.accessor('quantityOnHand', {
        header: 'Stock',
        cell: (c) => {
          const row = c.row.original;
          return (
            <span className={row.isLowStock ? 'font-medium text-amber-600' : ''}>
              {c.getValue()}
            </span>
          );
        },
      }),
      columnHelper.accessor('unitPrice', {
        header: 'Price',
        cell: (c) => `$${c.getValue().toFixed(2)}`,
      }),
      columnHelper.display({
        id: 'actions',
        header: 'Actions',
        cell: ({ row }) => (
          <div className="flex flex-wrap gap-2">
            <button
              type="button"
              className="text-sm text-brand-600 hover:underline"
              onClick={() => {
                setSelectedId(row.original.partId);
                setModal('edit');
              }}
            >
              Edit
            </button>
            <button
              type="button"
              className="text-sm text-brand-600 hover:underline"
              onClick={() => {
                setSelectedId(row.original.partId);
                setModal('adjust');
              }}
            >
              Stock
            </button>
            {isAdmin && (
              <button
                type="button"
                className="text-sm text-red-600 hover:underline"
                onClick={() => setDeleteId(row.original.partId)}
              >
                Delete
              </button>
            )}
          </div>
        ),
      }),
    ],
    [isAdmin],
  );

  const closeModal = () => {
    setModal(null);
    setSelectedId(null);
  };

  return (
    <>
      <PageHeader
        title="Parts Inventory"
        description="TanStack Table, React Hook Form, and Zod validation."
        actions={
          <button
            type="button"
            className="rounded-md bg-brand-600 px-4 py-2 text-sm font-medium text-white hover:bg-brand-900"
            onClick={() => setModal('create')}
          >
            New part
          </button>
        }
      />

      <div className="mb-4 flex flex-wrap gap-3">
        <input
          className="rounded-md border border-slate-300 px-3 py-2 text-sm"
          placeholder="Search SKU or name"
          value={search}
          onChange={(e) => {
            setSearch(e.target.value);
            setPage(1);
          }}
        />
        <select
          className="rounded-md border border-slate-300 px-3 py-2 text-sm"
          value={categoryId ?? ''}
          onChange={(e) => {
            setCategoryId(e.target.value ? Number(e.target.value) : undefined);
            setPage(1);
          }}
        >
          <option value="">All categories</option>
          {categories.data?.map((c) => (
            <option key={c.categoryId} value={c.categoryId}>
              {c.name}
            </option>
          ))}
        </select>
        <label className="flex items-center gap-2 text-sm text-slate-700">
          <input
            type="checkbox"
            checked={lowStockOnly}
            onChange={(e) => {
              setLowStockOnly(e.target.checked);
              setPage(1);
            }}
          />
          Low stock only
        </label>
      </div>

      {isLoading && <p className="text-slate-500">Loading parts…</p>}
      {isError && <p className="text-red-600">Failed to load parts.</p>}

      {data && (
        <div className="rounded-lg border border-slate-200 bg-white shadow-sm">
          <DataTable data={data.items} columns={columns} />
          <div className="flex items-center justify-between border-t px-4 py-3 text-sm text-slate-600">
            <span>
              Page {data.page} of {data.totalPages} ({data.totalCount} parts)
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
                disabled={page >= data.totalPages}
                className="rounded border px-2 py-1 disabled:opacity-40"
                onClick={() => setPage((p) => p + 1)}
              >
                Next
              </button>
            </div>
          </div>
        </div>
      )}

      <Modal open={modal === 'create'} title="New part" onClose={closeModal}>
        {categories.data && (
          <PartForm
            mode="create"
            categories={categories.data}
            onSubmit={(values) => {
              if ('initialQuantity' in values) {
                createPart.mutate(values, { onSuccess: closeModal });
              }
            }}
            onCancel={closeModal}
            isPending={createPart.isPending}
          />
        )}
      </Modal>

      <Modal open={modal === 'edit' && Boolean(selectedPart.data)} title="Edit part" onClose={closeModal}>
        {categories.data && selectedPart.data && (
          <PartForm
            mode="edit"
            categories={categories.data}
            defaultValues={{
              sku: selectedPart.data.sku,
              name: selectedPart.data.name,
              description: selectedPart.data.description ?? '',
              categoryId: selectedPart.data.categoryId,
              unitPrice: selectedPart.data.unitPrice,
              reorderLevel: selectedPart.data.reorderLevel,
            }}
            onSubmit={(values) =>
              updatePart.mutate(
                { id: selectedPart.data!.partId, body: values },
                { onSuccess: closeModal },
              )
            }
            onCancel={closeModal}
            isPending={updatePart.isPending}
          />
        )}
      </Modal>

      <Modal open={modal === 'adjust'} title="Adjust stock" onClose={closeModal}>
        {selectedPart.data && (
          <AdjustStockForm
            partName={selectedPart.data.name}
            onSubmit={(values) =>
              adjustStock.mutate(
                {
                  partId: selectedPart.data!.partId,
                  quantityChange: values.quantityChange,
                  reason: values.reason,
                },
                { onSuccess: closeModal },
              )
            }
            onCancel={closeModal}
            isPending={adjustStock.isPending}
          />
        )}
      </Modal>

      <ConfirmDialog
        open={deleteId !== null}
        title="Delete part"
        message="Soft-delete this part? It will be hidden from inventory lists."
        confirmLabel="Delete"
        loading={deletePart.isPending}
        onCancel={() => setDeleteId(null)}
        onConfirm={() => {
          if (deleteId) {
            deletePart.mutate(deleteId, { onSuccess: () => setDeleteId(null) });
          }
        }}
      />
    </>
  );
}
