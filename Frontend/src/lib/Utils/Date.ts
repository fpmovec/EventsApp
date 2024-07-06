export const excludeTimeFromISODate = (dateString: Date): string =>
  dateString.toISOString().split("T")[0];
