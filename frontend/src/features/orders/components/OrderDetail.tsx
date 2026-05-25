import type { Order } from '@/shared/types/api';

type OrderDetailProps = {
  order: Order;
};

export function OrderDetail({ order }: OrderDetailProps) {
  return (
    <div className="space-y-4 text-sm">
      <dl className="grid grid-cols-2 gap-2">
        <dt className="text-slate-500">Order #</dt>
        <dd className="font-mono">{order.orderNumber}</dd>
        <dt className="text-slate-500">Status</dt>
        <dd>{order.status}</dd>
        <dt className="text-slate-500">Total</dt>
        <dd className="font-medium">${order.totalAmount.toFixed(2)}</dd>
        <dt className="text-slate-500">Created</dt>
        <dd>{new Date(order.createdAt).toLocaleString()}</dd>
      </dl>
      <table className="min-w-full border text-left">
        <thead className="bg-slate-50 text-slate-600">
          <tr>
            <th className="px-3 py-2">SKU</th>
            <th className="px-3 py-2">Part</th>
            <th className="px-3 py-2">Qty</th>
            <th className="px-3 py-2">Unit</th>
            <th className="px-3 py-2">Line total</th>
          </tr>
        </thead>
        <tbody>
          {order.lines.map((line) => (
            <tr key={line.orderLineId} className="border-t">
              <td className="px-3 py-2 font-mono text-xs">{line.partSku}</td>
              <td className="px-3 py-2">{line.partName}</td>
              <td className="px-3 py-2">{line.quantity}</td>
              <td className="px-3 py-2">${line.unitPrice.toFixed(2)}</td>
              <td className="px-3 py-2">${line.lineTotal.toFixed(2)}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
