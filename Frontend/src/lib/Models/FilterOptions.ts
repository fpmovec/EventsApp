interface EventsFilterOptions {
  sortType: number;
  sortOrder: number;
  category: string;
  minDate: Date;
  maxDate: Date;
  minPrice: number;
  maxPrice: number;
  place: string;
  searchString: string;
  currentPage: number;
}

export type { EventsFilterOptions };
