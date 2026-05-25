import { zodResolver } from '@hookform/resolvers/zod';
import type { FieldValues, Resolver } from 'react-hook-form';

/** Bridges Zod 4 preprocess input types with react-hook-form's Resolver. */
export function formResolver<T extends FieldValues>(schema: object): Resolver<T> {
  // eslint-disable-next-line @typescript-eslint/no-explicit-any
  return zodResolver(schema as any) as Resolver<T>;
}
