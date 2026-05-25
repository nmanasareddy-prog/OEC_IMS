import { useForm } from 'react-hook-form';
import { formResolver } from '@/shared/lib/form-resolver';
import {
  createPartSchema,
  updatePartSchema,
  type CreatePartFormValues,
  type UpdatePartFormValues,
} from '@/features/parts/schemas/part.schema';
import { FormField, inputClass } from '@/shared/ui/FormField';
import type { Category } from '@/shared/types/api';

type PartFormProps = {
  mode: 'create' | 'edit';
  categories: Category[];
  defaultValues?: Partial<UpdatePartFormValues>;
  onSubmit: (values: CreatePartFormValues | UpdatePartFormValues) => void;
  onCancel: () => void;
  isPending?: boolean;
};

function CreatePartFormInner({
  categories,
  defaultValues,
  onSubmit,
  onCancel,
  isPending,
}: Omit<PartFormProps, 'mode'>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CreatePartFormValues>({
    resolver: formResolver(createPartSchema),
    defaultValues: {
      sku: '',
      name: '',
      description: '',
      categoryId: categories[0]?.categoryId ?? 1,
      unitPrice: 0,
      reorderLevel: 0,
      initialQuantity: 0,
      ...defaultValues,
    },
  });

  return (
    <form
      className="grid gap-4 md:grid-cols-2"
      onSubmit={handleSubmit((values) => onSubmit(values))}
    >
      <FormField label="SKU" error={errors.sku?.message}>
        <input className={inputClass} {...register('sku')} />
      </FormField>
      <FormField label="Name" error={errors.name?.message}>
        <input className={inputClass} {...register('name')} />
      </FormField>
      <FormField label="Category" error={errors.categoryId?.message}>
        <select className={inputClass} {...register('categoryId')}>
          {categories.map((c) => (
            <option key={c.categoryId} value={c.categoryId}>
              {c.name}
            </option>
          ))}
        </select>
      </FormField>
      <FormField label="Unit price" error={errors.unitPrice?.message}>
        <input type="number" step="0.01" className={inputClass} {...register('unitPrice')} />
      </FormField>
      <FormField label="Reorder level" error={errors.reorderLevel?.message}>
        <input type="number" className={inputClass} {...register('reorderLevel')} />
      </FormField>
      <FormField label="Initial quantity" error={errors.initialQuantity?.message}>
        <input type="number" className={inputClass} {...register('initialQuantity')} />
      </FormField>
      <FormField label="Description" error={errors.description?.message}>
        <input className={inputClass} {...register('description')} />
      </FormField>
      <div className="flex gap-2 md:col-span-2">
        <button
          type="submit"
          disabled={isPending}
          className="rounded-md bg-brand-600 px-4 py-2 text-sm text-white hover:bg-brand-900 disabled:opacity-60"
        >
          {isPending ? 'Saving…' : 'Save'}
        </button>
        <button type="button" onClick={onCancel} className="rounded-md border border-slate-300 px-4 py-2 text-sm">
          Cancel
        </button>
      </div>
    </form>
  );
}

function EditPartFormInner({
  categories,
  defaultValues,
  onSubmit,
  onCancel,
  isPending,
}: Omit<PartFormProps, 'mode'>) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdatePartFormValues>({
    resolver: formResolver(updatePartSchema),
    defaultValues: {
      sku: '',
      name: '',
      description: '',
      categoryId: categories[0]?.categoryId ?? 1,
      unitPrice: 0,
      reorderLevel: 0,
      ...defaultValues,
    },
  });

  return (
    <form
      className="grid gap-4 md:grid-cols-2"
      onSubmit={handleSubmit((values) => onSubmit(values))}
    >
      <FormField label="SKU" error={errors.sku?.message}>
        <input className={inputClass} {...register('sku')} />
      </FormField>
      <FormField label="Name" error={errors.name?.message}>
        <input className={inputClass} {...register('name')} />
      </FormField>
      <FormField label="Category" error={errors.categoryId?.message}>
        <select className={inputClass} {...register('categoryId')}>
          {categories.map((c) => (
            <option key={c.categoryId} value={c.categoryId}>
              {c.name}
            </option>
          ))}
        </select>
      </FormField>
      <FormField label="Unit price" error={errors.unitPrice?.message}>
        <input type="number" step="0.01" className={inputClass} {...register('unitPrice')} />
      </FormField>
      <FormField label="Reorder level" error={errors.reorderLevel?.message}>
        <input type="number" className={inputClass} {...register('reorderLevel')} />
      </FormField>
      <FormField label="Description" error={errors.description?.message}>
        <input className={inputClass} {...register('description')} />
      </FormField>
      <div className="flex gap-2 md:col-span-2">
        <button
          type="submit"
          disabled={isPending}
          className="rounded-md bg-brand-600 px-4 py-2 text-sm text-white hover:bg-brand-900 disabled:opacity-60"
        >
          {isPending ? 'Saving…' : 'Save'}
        </button>
        <button type="button" onClick={onCancel} className="rounded-md border border-slate-300 px-4 py-2 text-sm">
          Cancel
        </button>
      </div>
    </form>
  );
}

export function PartForm({ mode, ...props }: PartFormProps) {
  if (mode === 'create') {
    return <CreatePartFormInner {...props} />;
  }
  return <EditPartFormInner {...props} />;
}
