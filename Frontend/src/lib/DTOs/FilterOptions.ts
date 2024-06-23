interface EventsFilterOptions {
  sortType: 0 | 1 | 2 | 3;
  sortOrder: 0 | 1;
  category?: string;
  minDate?: Date | null;
  maxDate?: Date | null;
  minPrice?: number;
  maxPrice?: number;
  place?: string;
  currentPage: number;
}

export type { EventsFilterOptions };
