import EventBrief from "../../Components/EventItem/EventItem";
import Filters from "../../Components/Filters/Filters";
import styles from "./AllEvents.module.scss";
import { Pagination, PaginationItem } from "@mui/material";
import { ArrowBack, ArrowForward } from "@mui/icons-material";
import { useSearchParams, useNavigate } from "react-router-dom";
import { EventsFilterOptions } from "../../lib/DTOs/FilterOptions";
import { useState } from "react";
import { maxEventPrice } from "../../lib/Constants";

const AllEvents = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const pageFromQuery = Number(searchParams.get("currentPage"));

  const [filterOptions, setFilterOptions] = useState<EventsFilterOptions>({
    sortType: Number(searchParams.get("sortType")) ?? 0,
    sortOrder: Number(searchParams.get("sortOrder")) ?? 0,
    category: searchParams.get("category") ?? "",
    minDate: new Date(searchParams.get("minDate") ?? "") ?? Date.now(),
    maxDate: new Date(searchParams.get("maxDate") ?? "") ?? Date.now() + 365,
    minPrice: Number(searchParams.get("minPrice")) ?? 0,
    maxPrice: Number(searchParams.get("maxPrice")) ?? maxEventPrice,
    place: searchParams.get("place") ?? "",
    currentPage: pageFromQuery == 0 ? 1 : pageFromQuery,
    searchString: searchParams.get("searchString") ?? "",
  });

  const navigate = useNavigate();
  const page = pageFromQuery == 0 ? 1 : pageFromQuery;
  //const [currentPage, setPage] = useState<number>(page);

  return (
    <div>
      <div className={styles.filters}>
        <Filters
          filterOptions={filterOptions}
          setFilterOptions={setFilterOptions}
        />
      </div>
      <div className={styles.eventsList}>
        <EventBrief />
        <EventBrief />
        <EventBrief />
      </div>
      <div className={styles.pagination}>
        <Pagination
          count={10}
          defaultPage={1}
          onChange={(e, p) => {
            setFilterOptions((prev) => ({ ...prev, currentPage: p }));
            /* navigate(
              `/events?searchString=${filterOptions.searchString}&sortType=${filterOptions.sortType}&sortOrder=${filterOptions.sortOrder}&category=${filterOptions.category}&place=${filterOptions.place}&minDate=${filterOptions.minDate}&maxDate=${filterOptions.maxDate}&minPrice=${filterOptions.minPrice}&maxPrice=${filterOptions.maxPrice}&currentPage=${p}`
            ); */
            setSearchParams((prevParams) => {
              return new URLSearchParams({
                ...Object.fromEntries(prevParams.entries()),
                currentPage: p.toString(),
              });
            });
          }}
          page={page}
          renderItem={(item) => (
            <PaginationItem
              slots={{ previous: ArrowBack, next: ArrowForward }}
              {...item}
              size="large"
              color="primary"
            />
          )}
        />
      </div>
    </div>
  );
};

export default AllEvents;
