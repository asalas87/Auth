export const executeWithErrorHandling = async <T>(
  operation: () => Promise<T>,
  onSuccess?: (result: T) => void,
  onError?: () => void
): Promise<void> => {
  try {
    const result = await operation();
    onSuccess?.(result);
  } catch {
    onError?.();
  }
};