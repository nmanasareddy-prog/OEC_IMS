import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { apiFetch } from '@/shared/api/client';
import type { CompatiblePart, Manufacturer, VehicleModel } from '@/shared/types/api';

export function useManufacturers() {
  return useQuery({
    queryKey: ['vehicles', 'manufacturers'],
    queryFn: () => apiFetch<Manufacturer[]>('/api/v1/vehicles/manufacturers'),
  });
}

export function useVehicleModels(manufacturerId?: number) {
  return useQuery({
    queryKey: ['vehicles', 'models', manufacturerId],
    queryFn: () =>
      apiFetch<VehicleModel[]>(`/api/v1/vehicles/models?manufacturerId=${manufacturerId}`),
    enabled: Boolean(manufacturerId),
  });
}

export function useCompatibleParts(vehicleModelId?: number, year?: number) {
  return useQuery({
    queryKey: ['vehicles', 'compatible', vehicleModelId, year],
    queryFn: () =>
      apiFetch<CompatiblePart[]>(
        `/api/v1/vehicles/compatible-parts?vehicleModelId=${vehicleModelId}&year=${year}`,
      ),
    enabled: Boolean(vehicleModelId && year),
  });
}

export function useLinkCompatibility() {
  const qc = useQueryClient();
  return useMutation({
    mutationFn: (body: { partId: number; vehicleModelId: number; yearFrom: number; yearTo: number }) =>
      apiFetch('/api/v1/vehicles/compatibility', { method: 'POST', body: JSON.stringify(body) }),
    onSuccess: () => qc.invalidateQueries({ queryKey: ['vehicles'] }),
  });
}
