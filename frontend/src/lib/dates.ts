export function parseDates<T>(item: T, dateFields: (keyof T)[]): T {
  return {
    ...item,
    ...Object.fromEntries(
      dateFields.map(field => [
        field,
        item[field] ? new Date(item[field] as any) : null
      ])
    )
  };
}