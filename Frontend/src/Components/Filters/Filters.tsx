import { EventsFilterOptions } from "../../lib/Models/FilterOptions";
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
import { useAppSelector } from "../../lib/Redux/Hooks";

type Props = {
  filterOptions: EventsFilterOptions;
  setFilterOptions: (value: React.SetStateAction<EventsFilterOptions>) => void;
  onApply: () => void;
};

const Filters = ({ filterOptions, setFilterOptions, onApply }: Props) => {
  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  const [searchParams, setSearchParams] = useSearchParams();

  const [isExpanded, setIsExpanded] = useState<boolean>(false);

  const handleExpand = () => setIsExpanded((prev) => !prev);
  const categories = useAppSelector((state) => state.auth.categories);
  const [sortType, setSortType] = useState<string>("Default");
  const dateInYear = new Date();
  dateInYear.setDate(dateInYear.getDate() + 365 * 2);

  const minDate = ({
    v = filterOptions.minDate,
  }: { v?: Date | null | undefined } = {}) => {
    if (!isFinite(+(v as Date))) {
      return new Date();
    } else return v as Date;
  };

  const maxDate = ({
    v = filterOptions.maxDate,
  }: { v?: Date | null | undefined } = {}) => {
    if (!isFinite(+(v as Date))) {
      return dateInYear;
    } else return v as Date;
  };

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
          onClick={() => {
            setSearchParams((prevParams) => {
              return new URLSearchParams({
                ...Object.fromEntries(prevParams.entries()),
                searchString: filterOptions.searchString ?? "",
              });
            });
            onApply();
          }}
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
                valueDate={minDate()}
                handleValue={(value) =>
                  setFilterOptions((prev) => ({
                    ...prev,
                    minDate: minDate({ v: value }),
                  }))
                }
              />
              --
              <Calendar
                valueDate={maxDate()}
                handleValue={(value) =>
                  setFilterOptions((prev) => ({
                    ...prev,
                    maxDate: maxDate({ v: value }),
                  }))
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
                source={categories}
                handleValue={(v) =>
                  setFilterOptions((prev) => ({ ...prev, category: v }))
                }
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Price range:</h6>
              <Slider
                value={[
                  filterOptions.minPrice ?? minEventPrice,
                  filterOptions.maxPrice === 0
                    ? maxEventPrice
                    : (filterOptions.maxPrice as number),
                ]}
                onChange={(e, v) => handleChangeSlider(e, v)}
                valueLabelDisplay="on"
                defaultValue={[minEventPrice, maxEventPrice]}
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
                      minDate: minDate().toDateString(),
                      maxDate: maxDate().toDateString(),
                      minPrice:
                        filterOptions.minPrice?.toString() ??
                        minEventPrice.toString(),
                      maxPrice:
                        filterOptions.maxPrice?.toString() ??
                        maxEventPrice.toString(),
                    });
                  });
                  setIsExpanded(false);
                  onApply();
                }}
              />
              <WhiteButton
                text="Clear all"
                onClick={() => {
                  clearFilters();
                  setSearchParams(() => {
                    return new URLSearchParams({
                      searchString: filterOptions.searchString ?? "",
                      currentPage: currentPage.toString(),
                    });
                  });
                  setIsExpanded(false);
                  onApply();
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
