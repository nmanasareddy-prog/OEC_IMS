import { useMemo, useState } from 'react';
import { createColumnHelper } from '@tanstack/react-table';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import {
  useCompatibleParts,
  useLinkCompatibility,
  useManufacturers,
  useVehicleModels,
} from '@/features/vehicles/api/use-vehicles';
import { useParts } from '@/features/parts/api/use-parts';
import { PageHeader } from '@/shared/ui/PageHeader';
import { Card, CardHeader } from '@/shared/ui/Card';
import { DataTable } from '@/shared/ui/DataTable';
import { Button } from '@/shared/ui/Button';
import { FormField, inputClass } from '@/shared/ui/FormField';
import { LoadingSpinner } from '@/shared/ui/LoadingSpinner';
import { EmptyState } from '@/shared/ui/EmptyState';
import { formResolver } from '@/shared/lib/form-resolver';
import type { CompatiblePart } from '@/shared/types/api';

const linkSchema = z.object({
  partId: z.coerce.number().int().positive(),
  vehicleModelId: z.coerce.number().int().positive(),
  yearFrom: z.coerce.number().int().min(1980).max(2100),
  yearTo: z.coerce.number().int().min(1980).max(2100),
});

type LinkFormValues = z.infer<typeof linkSchema>;

const columnHelper = createColumnHelper<CompatiblePart>();

export function VehiclesPage() {
  const [manufacturerId, setManufacturerId] = useState<number>();
  const [vehicleModelId, setVehicleModelId] = useState<number>();
  const [year, setYear] = useState(2022);

  const manufacturers = useManufacturers();
  const models = useVehicleModels(manufacturerId);
  const compatible = useCompatibleParts(vehicleModelId, year);
  const parts = useParts({ page: 1, pageSize: 100 });
  const link = useLinkCompatibility();

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<LinkFormValues>({
    resolver: formResolver(linkSchema),
    defaultValues: { yearFrom: 2018, yearTo: 2024 },
  });

  const columns = useMemo(
    () => [
      columnHelper.accessor('sku', {
        header: 'SKU',
        cell: (c) => <span className="font-mono text-xs">{c.getValue()}</span>,
      }),
      columnHelper.accessor('name', { header: 'Part name' }),
      columnHelper.accessor('quantityOnHand', { header: 'Stock' }),
      columnHelper.display({
        id: 'years',
        header: 'Fitment years',
        cell: ({ row }) => `${row.original.yearFrom}–${row.original.yearTo}`,
      }),
    ],
    [],
  );

  return (
    <>
      <PageHeader
        title="Vehicle compatibility"
        description="Search fitment by manufacturer, model, and year. Link new parts with validated forms."
      />

      <Card className="mb-6">
        <div className="flex flex-wrap gap-3">
          <select
            className={inputClass}
            value={manufacturerId ?? ''}
            onChange={(e) => {
              setManufacturerId(e.target.value ? Number(e.target.value) : undefined);
              setVehicleModelId(undefined);
            }}
          >
            <option value="">Select manufacturer</option>
            {manufacturers.data?.map((m) => (
              <option key={m.manufacturerId} value={m.manufacturerId}>
                {m.name}
              </option>
            ))}
          </select>
          <select
            className={inputClass}
            value={vehicleModelId ?? ''}
            onChange={(e) => setVehicleModelId(e.target.value ? Number(e.target.value) : undefined)}
            disabled={!manufacturerId}
          >
            <option value="">Select model</option>
            {models.data?.map((m) => (
              <option key={m.vehicleModelId} value={m.vehicleModelId}>
                {m.name}
              </option>
            ))}
          </select>
          <input
            type="number"
            className={`${inputClass} w-28`}
            value={year}
            onChange={(e) => setYear(Number(e.target.value))}
            aria-label="Model year"
          />
        </div>
      </Card>

      {compatible.isLoading && <LoadingSpinner />}
      {vehicleModelId && compatible.data && (
        <Card className="mb-8" padding={false}>
          <CardHeader
            title="Compatible parts"
            description={`Parts that fit the selected vehicle for year ${year}`}
          />
          {compatible.data.length === 0 ? (
            <EmptyState title="No compatible parts" description="Try another year or link a part below." />
          ) : (
            <DataTable data={compatible.data} columns={columns} />
          )}
        </Card>
      )}

      <Card>
        <CardHeader title="Link part to vehicle" description="Creates a new fitment record in the catalog" />
        <form
          className="grid gap-4 md:grid-cols-2 lg:grid-cols-5"
          onSubmit={handleSubmit((values) => {
            link.mutate(values, {
              onSuccess: () => reset({ yearFrom: 2018, yearTo: 2024 }),
            });
          })}
        >
          <FormField label="Part" error={errors.partId?.message}>
            <select className={inputClass} {...register('partId')}>
              <option value="">Select part</option>
              {parts.data?.items.map((p) => (
                <option key={p.partId} value={p.partId}>
                  {p.sku} — {p.name}
                </option>
              ))}
            </select>
          </FormField>
          <FormField label="Vehicle model" error={errors.vehicleModelId?.message}>
            <select className={inputClass} {...register('vehicleModelId')}>
              <option value="">Select model</option>
              {models.data?.map((m) => (
                <option key={m.vehicleModelId} value={m.vehicleModelId}>
                  {m.name}
                </option>
              ))}
            </select>
          </FormField>
          <FormField label="Year from" error={errors.yearFrom?.message}>
            <input type="number" className={inputClass} {...register('yearFrom')} />
          </FormField>
          <FormField label="Year to" error={errors.yearTo?.message}>
            <input type="number" className={inputClass} {...register('yearTo')} />
          </FormField>
          <div className="flex items-end">
            <Button type="submit" disabled={link.isPending}>
              {link.isPending ? 'Linking…' : 'Link part'}
            </Button>
          </div>
        </form>
        {link.isSuccess && (
          <p className="mt-3 text-sm text-emerald-600">Fitment linked. Refresh compatible list above.</p>
        )}
      </Card>
    </>
  );
}
