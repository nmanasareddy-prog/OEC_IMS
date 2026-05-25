import { useForm } from 'react-hook-form';
import { formResolver } from '@/shared/lib/form-resolver';
import {
  adjustStockSchema,
  type AdjustStockFormValues,
} from '@/features/parts/schemas/part.schema';
import { FormField, inputClass } from '@/shared/ui/FormField';

type AdjustStockFormProps = {
  partName: string;
  onSubmit: (values: AdjustStockFormValues) => void;
  onCancel: () => void;
  isPending?: boolean;
};

export function AdjustStockForm({ partName, onSubmit, onCancel, isPending }: AdjustStockFormProps) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<AdjustStockFormValues>({
    resolver: formResolver(adjustStockSchema),
    defaultValues: { quantityChange: 1, reason: '' },
  });

  return (
    <form className="space-y-4" onSubmit={handleSubmit(onSubmit)}>
      <p className="text-sm text-slate-600">
        Adjust stock for <strong>{partName}</strong>. Use negative values to reduce.
      </p>
      <FormField label="Quantity change" error={errors.quantityChange?.message}>
        <input type="number" className={inputClass} {...register('quantityChange')} />
      </FormField>
      <FormField label="Reason" error={errors.reason?.message}>
        <input className={inputClass} {...register('reason')} placeholder="Optional" />
      </FormField>
      <div className="flex gap-2">
        <button
          type="submit"
          disabled={isPending}
          className="rounded-md bg-brand-600 px-4 py-2 text-sm text-white disabled:opacity-60"
        >
          {isPending ? 'Applying…' : 'Apply'}
        </button>
        <button type="button" onClick={onCancel} className="rounded-md border px-4 py-2 text-sm">
          Cancel
        </button>
      </div>
    </form>
  );
}
