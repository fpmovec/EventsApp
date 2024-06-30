import EventBrief from "../../Components/EventItem/EventItem";
import Filters from "../../Components/Filters/Filters";
import styles from "./AllEvents.module.scss";
import { Pagination, PaginationItem } from "@mui/material";
import { ArrowBack, ArrowForward } from "@mui/icons-material";
import { useSearchParams, useNavigate } from "react-router-dom";
import { EventsFilterOptions } from "../../lib/DTOs/FilterOptions";
import { useEffect, useState } from "react";
import { maxEventPrice } from "../../lib/Constants";
import { EventItem } from "../../lib/DTOs/Event";
import { GetEvents, GetPagesCount } from "../../lib/Requests/GET/EventsRequests";

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

  const [eventItems, setEventItems] = useState<EventItem[]>([]);
const [pagesCount, setPagesCount] = useState<number>(1);
  const getEvents = async () => {
    const items = await GetEvents(filterOptions);
    setEventItems(items);
  };

  useEffect(() => {
    const getPagesCount = async () => {
      const pages = await GetPagesCount();
      setPagesCount(pages);
    }
    getEvents();

    getPagesCount();
  }, [pageFromQuery]);

  return (
    <div>
      <div className={styles.filters}>
        <Filters
          filterOptions={filterOptions}
          setFilterOptions={setFilterOptions}
          onApply={getEvents}
        />
      </div>
      {eventItems.length > 0 ? (
        <>
          <div className={styles.eventsList}>
            {eventItems.map((e, i) => (
              <EventBrief eventItem={e} key={i} />
            ))}
          </div>
          <div className={styles.pagination}>
            <Pagination
              count={pagesCount}
              defaultPage={1}
              onChange={(e, p) => {
                setFilterOptions((prev) => ({ ...prev, currentPage: p }));
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
        </>
      ) : (
        <h4
          style={{
            fontWeight: 300,
            letterSpacing: 2,
            textAlign: "center",
            marginTop: 35,
            marginBottom: 350,
          }}
        >
          No events found
        </h4>
      )}
    </div>
  );
};

export default AllEvents;
