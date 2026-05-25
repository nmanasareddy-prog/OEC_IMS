export const partKeys = {
  all: ['parts'] as const,
  list: (filters: Record<string, unknown>) => [...partKeys.all, 'list', filters] as const,
  detail: (id: number) => [...partKeys.all, 'detail', id] as const,
  categories: ['parts', 'categories'] as const,
};
