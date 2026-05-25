import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiFetch } from '@/shared/api/client';
import type { Order, OrderListItem, PagedResult } from '@/shared/types/api';
import type { CreateOrderFormValues } from '@/features/orders/schemas/order.schema';

export const orderKeys = {
  all: ['orders'] as const,
  list: (page: number) => [...orderKeys.all, 'list', page] as const,
  detail: (id: number) => [...orderKeys.all, 'detail', id] as const,
};

export function useOrders(page = 1) {
  return useQuery({
    queryKey: orderKeys.list(page),
    queryFn: () => apiFetch<PagedResult<OrderListItem>>(`/api/v1/orders?page=${page}&pageSize=20`),
  });
}

export function useOrder(orderId: number | undefined) {
  return useQuery({
    queryKey: orderKeys.detail(orderId ?? 0),
    queryFn: () => apiFetch<Order>(`/api/v1/orders/${orderId}`),
    enabled: Boolean(orderId),
  });
}

export function useCreateOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (body: CreateOrderFormValues) =>
      apiFetch<Order>('/api/v1/orders', {
        method: 'POST',
        body: JSON.stringify(body),
      }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: orderKeys.all });
      qc.invalidateQueries({ queryKey: ['parts'] });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}

export function useCancelOrder() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (orderId: number) =>
      apiFetch<Order>(`/api/v1/orders/${orderId}/cancel`, { method: 'POST' }),
    onSuccess: () => {
      qc.invalidateQueries({ queryKey: orderKeys.all });
      qc.invalidateQueries({ queryKey: ['parts'] });
      qc.invalidateQueries({ queryKey: ['dashboard'] });
    },
  });
}
