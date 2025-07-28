export type ExtractionPatterns = Record<string, RegExp>;

export function extractDataFromText(text: string, patterns: ExtractionPatterns): Record<string, string> {
  const result: Record<string, string> = {};

  for (const key in patterns) {
    const match = text.match(patterns[key]);
    result[key] = match?.[1]?.trim() ?? '';
  }

  return result;
}
