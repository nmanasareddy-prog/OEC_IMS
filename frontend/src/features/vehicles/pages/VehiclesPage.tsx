import { useState } from 'react';
import {
  useCompatibleParts,
  useLinkCompatibility,
  useManufacturers,
  useVehicleModels,
} from '@/features/vehicles/api/use-vehicles';
import { useParts } from '@/features/parts/api/use-parts';
import { PageHeader } from '@/shared/ui/PageHeader';

export function VehiclesPage() {
  const [manufacturerId, setManufacturerId] = useState<number>();
  const [vehicleModelId, setVehicleModelId] = useState<number>();
  const [year, setYear] = useState(2022);

  const manufacturers = useManufacturers();
  const models = useVehicleModels(manufacturerId);
  const compatible = useCompatibleParts(vehicleModelId, year);
  const parts = useParts({ page: 1, pageSize: 100 });
  const link = useLinkCompatibility();

  return (
    <>
      <PageHeader title="Vehicle Compatibility" description="Find and manage parts fitment by vehicle." />
      <div className="mb-4 flex flex-wrap gap-3">
        <select
          className="rounded-md border px-3 py-2 text-sm"
          value={manufacturerId ?? ''}
          onChange={(e) => {
            setManufacturerId(Number(e.target.value));
            setVehicleModelId(undefined);
          }}
        >
          <option value="">Manufacturer</option>
          {manufacturers.data?.map((m) => (
            <option key={m.manufacturerId} value={m.manufacturerId}>{m.name}</option>
          ))}
        </select>
        <select
          className="rounded-md border px-3 py-2 text-sm"
          value={vehicleModelId ?? ''}
          onChange={(e) => setVehicleModelId(Number(e.target.value))}
          disabled={!manufacturerId}
        >
          <option value="">Model</option>
          {models.data?.map((m) => (
            <option key={m.vehicleModelId} value={m.vehicleModelId}>{m.name}</option>
          ))}
        </select>
        <input
          type="number"
          className="w-24 rounded-md border px-3 py-2 text-sm"
          value={year}
          onChange={(e) => setYear(Number(e.target.value))}
        />
      </div>

      {compatible.data && (
        <div className="mb-8 rounded-lg border bg-white shadow-sm">
          <h3 className="border-b px-4 py-3 text-sm font-semibold">Compatible parts</h3>
          <ul className="divide-y">
            {compatible.data.map((p) => (
              <li key={p.partId} className="flex justify-between px-4 py-3 text-sm">
                <span>{p.sku} — {p.name}</span>
                <span>Stock: {p.quantityOnHand} · {p.yearFrom}–{p.yearTo}</span>
              </li>
            ))}
            {compatible.data.length === 0 && (
              <li className="px-4 py-6 text-center text-slate-500">No compatible parts for this vehicle/year.</li>
            )}
          </ul>
        </div>
      )}

      <div className="rounded-lg border bg-white p-4 shadow-sm">
        <h3 className="text-sm font-semibold text-slate-700">Link part to vehicle</h3>
        <form
          className="mt-3 flex flex-wrap gap-3"
          onSubmit={(e) => {
            e.preventDefault();
            const fd = new FormData(e.currentTarget);
            link.mutate({
              partId: Number(fd.get('partId')),
              vehicleModelId: Number(fd.get('vehicleModelId')),
              yearFrom: Number(fd.get('yearFrom')),
              yearTo: Number(fd.get('yearTo')),
            });
          }}
        >
          <select name="partId" className="rounded border px-3 py-2 text-sm" required>
            {parts.data?.items.map((p) => (
              <option key={p.partId} value={p.partId}>{p.sku} — {p.name}</option>
            ))}
          </select>
          <select name="vehicleModelId" className="rounded border px-3 py-2 text-sm" required>
            {models.data?.map((m) => (
              <option key={m.vehicleModelId} value={m.vehicleModelId}>{m.name}</option>
            ))}
          </select>
          <input name="yearFrom" type="number" placeholder="From" className="w-24 rounded border px-3 py-2 text-sm" required />
          <input name="yearTo" type="number" placeholder="To" className="w-24 rounded border px-3 py-2 text-sm" required />
          <button type="submit" className="rounded-md bg-brand-600 px-4 py-2 text-sm text-white">Link</button>
        </form>
      </div>
    </>
  );
}
