export type PagedResult<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
};

export type LoginResponse = {
  token: string;
  userId: string;
  username: string;
  roles: string[];
};

export type Category = { categoryId: number; name: string };

export type PartListItem = {
  partId: number;
  sku: string;
  name: string;
  categoryName: string;
  unitPrice: number;
  quantityOnHand: number;
  reorderLevel: number;
  isLowStock: boolean;
};

export type Part = PartListItem & {
  description?: string | null;
  categoryId: number;
};

export type DashboardMetrics = {
  totalParts: number;
  lowStockCount: number;
  pendingOrdersCount: number;
  recentActivity: {
    stockMovementId: number;
    partSku: string;
    partName: string;
    quantityChange: number;
    movementType: string;
    occurredAt: string;
  }[];
};

export type Manufacturer = { manufacturerId: number; name: string };
export type VehicleModel = { vehicleModelId: number; manufacturerId: number; name: string };

export type CompatiblePart = {
  partId: number;
  sku: string;
  name: string;
  quantityOnHand: number;
  yearFrom: number;
  yearTo: number;
};

export type OrderLine = {
  orderLineId: number;
  partId: number;
  partSku: string;
  partName: string;
  quantity: number;
  unitPrice: number;
  lineTotal: number;
};

export type OrderListItem = {
  orderId: number;
  orderNumber: string;
  status: string;
  totalAmount: number;
  createdAt: string;
  lineCount: number;
};

export type Order = OrderListItem & { lines: OrderLine[] };
