const SortType: Record<string, number> = {
  "Default": 0,
  "By date": 1,
  "By name": 2,
  "By price": 3,
};

const SortOrder: Record<string, number> = {
  Asc: 0,
  Desc: 1,
};

export { SortType, SortOrder };