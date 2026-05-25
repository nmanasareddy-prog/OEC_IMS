export function getRolesFromToken(token: string | null): string[] {
  if (!token) return [];
  try {
    const payload = token.split('.')[1];
    if (!payload) return [];
    const json = JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')));
    const role =
      json['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ??
      json.role;
    if (Array.isArray(role)) return role as string[];
    if (typeof role === 'string') return [role];
    return [];
  } catch {
    return [];
  }
}

export function hasRole(token: string | null, role: string): boolean {
  return getRolesFromToken(token).includes(role);
}
