import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiFetch } from '@/shared/api/client';
import type { Category, PagedResult, Part, PartListItem } from '@/shared/types/api';
import { partKeys } from '@/features/parts/api/query-keys';
import type { CreatePartFormValues, UpdatePartFormValues } from '@/features/parts/schemas/part.schema';

export type PartFilters = {
  page?: number;
  pageSize?: number;
  search?: string;
  categoryId?: number;
  lowStockOnly?: boolean;
};

function buildQuery(filters: PartFilters) {
  const params = new URLSearchParams();
  params.set('page', String(filters.page ?? 1));
  params.set('pageSize', String(filters.pageSize ?? 20));
  if (filters.search) params.set('search', filters.search);
  if (filters.categoryId) params.set('categoryId', String(filters.categoryId));
  if (filters.lowStockOnly) params.set('lowStockOnly', 'true');
  return params.toString();
}

export function useParts(filters: PartFilters) {
  return useQuery({
    queryKey: partKeys.list(filters),
    queryFn: () =>
      apiFetch<PagedResult<PartListItem>>(`/api/v1/parts?${buildQuery(filters)}`),
  });
}

export function usePart(id: number | undefined) {
  return useQuery({
    queryKey: partKeys.detail(id ?? 0),
    queryFn: () => apiFetch<Part>(`/api/v1/parts/${id}`),
    enabled: Boolean(id),
  });
}

export function useCategories() {
  return useQuery({
    queryKey: partKeys.categories,
    queryFn: () => apiFetch<Category[]>('/api/v1/parts/categories'),
  });
}

export function useCreatePart() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (body: CreatePartFormValues) =>
      apiFetch<Part>('/api/v1/parts', {
        method: 'POST',
        body: JSON.stringify({
          ...body,
          description: body.description || null,
        }),
      }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: partKeys.all });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}

export function useUpdatePart() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({ id, body }: { id: number; body: UpdatePartFormValues }) =>
      apiFetch<Part>(`/api/v1/parts/${id}`, {
        method: 'PUT',
        body: JSON.stringify({
          ...body,
          description: body.description || null,
        }),
      }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: partKeys.all });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}

export function useDeletePart() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (id: number) =>
      apiFetch<void>(`/api/v1/parts/${id}`, { method: 'DELETE' }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: partKeys.all });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}

export function useAdjustStock() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: ({
      partId,
      quantityChange,
      reason,
    }: {
      partId: number;
      quantityChange: number;
      reason?: string;
    }) =>
      apiFetch<Part>(`/api/v1/parts/${partId}/adjust-stock`, {
        method: 'POST',
        body: JSON.stringify({ quantityChange, reason: reason || null }),
      }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: partKeys.all });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}
