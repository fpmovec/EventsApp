interface EventsFilterOptions {
  sortType: number;
  sortOrder: number;
  category?: string | null;
  minDate?: Date | null;
  maxDate?: Date | null;
  minPrice?: number | null;
  maxPrice?: number | null;
  place?: string | null;
  searchString?: string | null;
  currentPage: number;
}

export type { EventsFilterOptions };
