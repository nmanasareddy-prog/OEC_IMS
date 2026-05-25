import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { formResolver } from '@/shared/lib/form-resolver';
import {
  createOrderSchema,
  orderLineSchema,
  type CreateOrderFormValues,
} from '@/features/orders/schemas/order.schema';
import { FormField, inputClass } from '@/shared/ui/FormField';
import type { PartListItem } from '@/shared/types/api';

type CreateOrderFormProps = {
  parts: PartListItem[];
  onSubmit: (values: CreateOrderFormValues) => void;
  isPending?: boolean;
};

export function CreateOrderForm({ parts, onSubmit, isPending }: CreateOrderFormProps) {
  const [draftPartId, setDraftPartId] = useState<number | ''>('');
  const [draftQty, setDraftQty] = useState(1);
  const [lineError, setLineError] = useState<string | null>(null);

  const {
    watch,
    setValue,
    handleSubmit,
    formState: { errors },
  } = useForm<CreateOrderFormValues>({
    resolver: formResolver(createOrderSchema),
    defaultValues: { lines: [] },
  });

  const lines = watch('lines');

  const addLine = () => {
    const parsed = orderLineSchema.safeParse({
      partId: draftPartId,
      quantity: draftQty,
    });
    if (!parsed.success) {
      setLineError(parsed.error.issues[0]?.message ?? 'Invalid line');
      return;
    }
    if (lines.some((l) => l.partId === parsed.data.partId)) {
      setLineError('Part already on this order');
      return;
    }
    setLineError(null);
    setValue('lines', [...lines, parsed.data]);
    setDraftPartId('');
    setDraftQty(1);
  };

  const removeLine = (index: number) => {
    setValue(
      'lines',
      lines.filter((_, i) => i !== index),
    );
  };

  const partLabel = (partId: number) => {
    const p = parts.find((x) => x.partId === partId);
    return p ? `${p.sku} — ${p.name}` : `Part #${partId}`;
  };

  return (
    <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
      <div className="flex flex-wrap items-end gap-3">
        <FormField label="Part" className="min-w-[220px] flex-1">
          <select
            className={inputClass}
            value={draftPartId}
            onChange={(e) => setDraftPartId(e.target.value ? Number(e.target.value) : '')}
          >
            <option value="">Select part</option>
            {parts.map((p) => (
              <option key={p.partId} value={p.partId}>
                {p.sku} — {p.name} (stock: {p.quantityOnHand})
              </option>
            ))}
          </select>
        </FormField>
        <FormField label="Qty" className="w-24">
          <input
            type="number"
            min={1}
            className={inputClass}
            value={draftQty}
            onChange={(e) => setDraftQty(Number(e.target.value))}
          />
        </FormField>
        <button
          type="button"
          onClick={addLine}
          className="rounded-md border border-slate-300 px-4 py-2 text-sm hover:bg-slate-50"
        >
          Add line
        </button>
      </div>
      {lineError && <p className="text-sm text-red-600">{lineError}</p>}

      {lines.length > 0 ? (
        <ul className="divide-y rounded-md border text-sm">
          {lines.map((line, index) => (
            <li key={`${line.partId}-${index}`} className="flex items-center justify-between px-3 py-2">
              <span>
                {partLabel(line.partId)} × {line.quantity}
              </span>
              <button
                type="button"
                className="text-red-600 hover:underline"
                onClick={() => removeLine(index)}
              >
                Remove
              </button>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-sm text-slate-500">No line items yet.</p>
      )}

      {errors.lines?.message && (
        <p className="text-sm text-red-600">{errors.lines.message}</p>
      )}

      <button
        type="submit"
        disabled={isPending || lines.length === 0}
        className="rounded-md bg-brand-600 px-4 py-2 text-sm text-white hover:bg-brand-900 disabled:opacity-50"
      >
        {isPending ? 'Placing order…' : 'Place order'}
      </button>
    </form>
  );
}
