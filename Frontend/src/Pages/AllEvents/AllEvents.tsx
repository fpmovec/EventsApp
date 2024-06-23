import EventBrief from "../../Components/EventItem/EventItem";
import Filters from "../../Components/Filters/Filters";
import styles from "./AllEvents.module.scss";
import { Pagination, PaginationItem } from "@mui/material";
import { ArrowBack, ArrowForward } from "@mui/icons-material";

const AllEvents = () => {
  return (
    <div>
      <div className={styles.filters}>
        <Filters />
      </div>
      <div className={styles.eventsList}>
        <EventBrief />
        <EventBrief />
        <EventBrief />
      </div>
      <div className={styles.pagination}>
        <Pagination
          count={10}
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
