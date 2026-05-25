import { z } from 'zod';

const numberField = (message: string) =>
  z.preprocess(
    (val) => (val === '' || val === undefined || val === null ? NaN : Number(val)),
    z.number({ error: message }),
  );

export const orderLineSchema = z.object({
  partId: numberField('Select a part').pipe(z.number().int().positive('Select a part')),
  quantity: numberField('Quantity must be at least 1').pipe(
    z.number().int().positive('Quantity must be at least 1'),
  ),
});

export const createOrderSchema = z.object({
  lines: z.array(orderLineSchema).min(1, 'Add at least one line item'),
});

export type CreateOrderFormValues = z.infer<typeof createOrderSchema>;
