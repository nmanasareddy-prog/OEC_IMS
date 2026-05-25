import { z } from 'zod';

const optionalText = z.string().max(500).optional().or(z.literal(''));

const numberField = (message: string) =>
  z.preprocess(
    (val) => (val === '' || val === undefined || val === null ? NaN : Number(val)),
    z.number({ error: message }),
  );

export const createPartSchema = z.object({
  sku: z.string().min(1, 'SKU is required').max(64),
  name: z.string().min(1, 'Name is required').max(256),
  description: optionalText,
  categoryId: numberField('Select a category').pipe(z.number().int().positive('Select a category')),
  unitPrice: numberField('Price must be 0 or greater').pipe(z.number().min(0, 'Price must be 0 or greater')),
  reorderLevel: numberField('Invalid reorder level').pipe(z.number().int().min(0)),
  initialQuantity: numberField('Invalid quantity').pipe(z.number().int().min(0)),
});

export type CreatePartFormValues = z.infer<typeof createPartSchema>;

export const updatePartSchema = createPartSchema.omit({ initialQuantity: true });

export type UpdatePartFormValues = z.infer<typeof updatePartSchema>;

export const adjustStockSchema = z.object({
  quantityChange: numberField('Enter a non-zero adjustment').pipe(
    z.number().int().refine((v) => v !== 0, 'Enter a non-zero adjustment'),
  ),
  reason: z.string().max(256).optional().or(z.literal('')),
});

export type AdjustStockFormValues = z.infer<typeof adjustStockSchema>;
