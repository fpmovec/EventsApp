import { EventsFilterOptions } from "../../lib/DTOs/FilterOptions";
import {
  KeyboardArrowDown,
  KeyboardArrowUp,
  ArrowUpward,
} from "@mui/icons-material";
import styles from "./Filters.module.scss";
import { useState } from "react";
import Calendar from "../Generic/Calendar/Calendar";
import Selector from "../Generic/Select/Selector";
import { Slider } from "@mui/material";
import { WhiteButton } from "../Generic/Button/Buttons";
import { TextField } from "@mui/material";
import { useSearchParams } from "react-router-dom";
import { SortType } from "../../lib/Utils/SortUtils";
import {
  maxEventPrice,
  minEventPrice,
  priceSliderStep,
} from "../../lib/Constants";

type Props = {
  filterOptions: EventsFilterOptions;
  setFilterOptions: (value: React.SetStateAction<EventsFilterOptions>) => void;
};

const Filters = ({ filterOptions, setFilterOptions }: Props) => {
  const [searchParams, setSearchParams] = useSearchParams();

  const [isExpanded, setIsExpanded] = useState<boolean>(false);

  const handleExpand = () => setIsExpanded((prev) => !prev);

  const [sortType, setSortType] = useState<string>("Default");
  const dateInYear = new Date();
  dateInYear.setDate(dateInYear.getDate() + 365);
  const currentPage = filterOptions.currentPage;
  const clearFilters = () =>
    setFilterOptions((prev) => ({
      ...prev,
      sortType: 0,
      sortOrder: 0,
      category: "",
      minDate: new Date(),
      maxDate: dateInYear,
      minPrice: 0,
      maxPrice: maxEventPrice,
      place: "",
      searchString: "",
    }));

  const handleChangeSlider = (
    event: React.SyntheticEvent | Event,
    newValue: number | Array<number>
  ) => {
    if (!Array.isArray(newValue)) return;
    setFilterOptions((prev) => ({
      ...prev,
      minPrice: newValue[0],
      maxPrice: newValue[1],
    }));
  };
  console.log(filterOptions);
  return (
    <div className={styles.main}>
      <div className={styles.search}>
        <TextField
          label="Search event by name..."
          id="outlined-size-small"
          defaultValue="Small"
          size="small"
          value={filterOptions.searchString}
          onChange={(v) => {
            setFilterOptions((prev) => ({
              ...prev,
              searchString: v.target.value,
            }));
          }}
        />
        <WhiteButton
          text="Search"
          onClick={() =>
            setSearchParams((prevParams) => {
              return new URLSearchParams({
                ...Object.fromEntries(prevParams.entries()),
                searchString: filterOptions.searchString ?? "",
              });
            })
          }
        />
      </div>
      <div className={styles.expander}>
        <div className={styles.expanderHeader} onClick={handleExpand}>
          <div className={styles.expanderTitle}>Filter and Sort</div>
          {isExpanded ? (
            <KeyboardArrowUp fontSize="large" />
          ) : (
            <KeyboardArrowDown fontSize="large" />
          )}
        </div>
        <div className={isExpanded ? styles.expanded : styles.collapsed}>
          <div className={styles.expanderBody}>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Date range:</h6>
              <Calendar
                valueDate={filterOptions.minDate ?? new Date()}
                handleValue={(v) =>
                  setFilterOptions((prev) => ({ ...prev, minDate: v ?? new Date() }))
                }
              />
              --
              <Calendar
                valueDate={filterOptions.maxDate ?? dateInYear}
                handleValue={(v) =>
                  setFilterOptions((prev) => ({ ...prev, maxDate: v ?? dateInYear }))
                }
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>City:</h6>
              <Selector
                label="City"
                value={filterOptions.place ?? ""}
                source={["Minsk", "Moscow", "Mogilev"]}
                handleValue={(v) =>
                  setFilterOptions((prev) => ({ ...prev, place: v }))
                }
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Category:</h6>
              <Selector
                label="Category"
                value={filterOptions.category as string}
                source={["Festival", "Concert", "Conference"]}
                handleValue={(v) =>
                  setFilterOptions((prev) => ({ ...prev, category: v }))
                }
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Price range:</h6>
              <Slider
                value={[
                  filterOptions.minPrice ?? 0,
                  filterOptions.maxPrice ?? 0,
                ]}
                onChange={(e, v) => handleChangeSlider(e, v)}
                valueLabelDisplay="on"
                disableSwap
                color="primary"
                min={minEventPrice}
                max={maxEventPrice}
                step={priceSliderStep}
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Sort:</h6>
              <Selector
                label="Sort by"
                value={sortType ?? ""}
                source={["Default", "By name", "By date", "By price"]}
                handleValue={(v) => {
                  setFilterOptions((prev) => ({
                    ...prev,
                    sortType: SortType[v],
                  }));
                  setSortType(v);
                }}
              />
              <div
                onClick={() =>
                  setFilterOptions((prev) => ({
                    ...prev,
                    sortOrder: prev.sortOrder == 1 ? 0 : 1,
                  }))
                }
                className={
                  filterOptions.sortOrder
                    ? styles.descendingOrder
                    : styles.ascendingOrder
                }
                style={{ marginLeft: 30 }}
              >
                <ArrowUpward color="primary" fontSize="large" />
              </div>
            </div>
            <div></div>
            <div className={styles.filterButtons}>
              <WhiteButton
                text="Apply"
                onClick={() => {
                  setSearchParams((prevParams) => {
                    return new URLSearchParams({
                      ...Object.fromEntries(prevParams.entries()),
                      sortType: filterOptions.sortType.toString(),
                      sortOrder: filterOptions.sortOrder.toString(),
                      category: filterOptions.category ?? "",
                      place: filterOptions.place ?? "",
                      minDate: (filterOptions.minDate ?? new Date()).toDateString(),
                      maxDate: (filterOptions.maxDate ?? dateInYear).toDateString(),
                      minPrice:
                        filterOptions.minPrice?.toString() ??
                        minEventPrice.toString(),
                      maxPrice:
                        filterOptions.maxPrice?.toString() ??
                        maxEventPrice.toString(),
                    });
                  });
                  setIsExpanded(false);
                }}
              />
              <WhiteButton
                text="Clear all"
                onClick={() => {
                  clearFilters();
                  /* navigate(
                    `/events?searchString=${filterOptions.searchString}&currentPage=${currentPage}`
                  ); */
                  setSearchParams(() => {
                    return new URLSearchParams({
                      searchString: filterOptions.searchString ?? "",
                      currentPage: currentPage.toString(),
                    });
                  });
                  setIsExpanded(false);
                }}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Filters;
