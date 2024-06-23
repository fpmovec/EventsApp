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
import { BlueButton, WhiteButton } from "../Generic/Button/Buttons";
import { TextField } from "@mui/material";

type Props = {
  filterOptions: EventsFilterOptions;
  setOptions: (arg: EventsFilterOptions) => void;
};

const Filters = () => {
  const [options, setOptions] = useState<EventsFilterOptions>({
    sortType: 0,
    sortOrder: 0,
    category: "",
    minDate: null,
    maxDate: null,
    minPrice: 0,
    maxPrice: 2147483647,
    place: "",
    currentPage: 1,
  });

  const [isExpanded, setIsExpanded] = useState<boolean>(false);
  const [city, setCity] = useState<string>("");
  const [category, setCategory] = useState<string>("");
  const [priceRange, setPriceRange] = useState<number[]>([10, 200]);
  const [sortType, setSortType] = useState<string>("Default");
  const [sortOrder, setSortOrder] = useState<boolean>(false);

  const handleExpand = () => setIsExpanded((prev) => !prev);
  const handleChangeSlider = (
    event: React.SyntheticEvent | Event,
    newValue: number | Array<number>
  ) => {
    if (!Array.isArray(newValue)) return;
    console.log(newValue);
    setPriceRange(newValue as number[]);
  };

  return (
    <div className={styles.main}>
      <div className={styles.search}>
        <TextField
          label="Search event by name..."
          id="outlined-size-small"
          defaultValue="Small"
          size="small"
        />
        <WhiteButton text="Search" onClick={() => console.log()}/>
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
              <Calendar handleValue={(v) => console.log(v)} />
              --
              <Calendar handleValue={(v) => console.log(v)} />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>City:</h6>
              <Selector
                label="City"
                value={city}
                source={["Minsk", "Moscow", "Mogilev"]}
                handleValue={setCity}
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Category:</h6>
              <Selector
                label="Category"
                value={category}
                source={["Festival", "Concert", "Conference"]}
                handleValue={setCategory}
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Price range:</h6>
              <Slider
                value={priceRange}
                onChangeCommitted={(e, v) => handleChangeSlider(e, v)}
                onChange={(e, v) => handleChangeSlider(e, v)}
                valueLabelDisplay="on"
                disableSwap
                color="primary"
                min={0}
                max={2000}
                step={5}
              />
            </div>
            <div className={styles.filterItem}>
              <h6 style={{ marginRight: 10 }}>Sort:</h6>
              <Selector
                label="Sort by"
                value={sortType}
                source={["Default", "By name", "By date"]}
                handleValue={setSortType}
              />
              <div
                onClick={() => setSortOrder((prev) => !prev)}
                className={
                  sortOrder ? styles.descendingOrder : styles.ascendingOrder
                }
                style={{ marginLeft: 30 }}
              >
                <ArrowUpward color="primary" fontSize="large" />
              </div>
            </div>
            <div></div>
            <div className={styles.filterButtons}>
              <WhiteButton text="Apply" onClick={() => console.log()} />
              <WhiteButton text="Clear all" onClick={() => console.log()} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Filters;
