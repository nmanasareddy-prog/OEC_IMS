import { useQuery } from '@tanstack/react-query';
import { apiFetch } from '@/shared/api/client';
import type { DashboardMetrics } from '@/shared/types/api';

export function useDashboardMetrics() {
  return useQuery({
    queryKey: ['dashboard', 'metrics'],
    queryFn: () => apiFetch<DashboardMetrics>('/api/v1/dashboard/metrics'),
  });
}
