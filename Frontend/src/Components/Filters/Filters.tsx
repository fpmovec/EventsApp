import { EventsFilterOptions } from "../../lib/DTOs/FilterOptions";
import { KeyboardArrowDown, KeyboardArrowUp } from "@mui/icons-material";
import styles from "./Filters.module.scss";
import { useState } from "react";
import Calendar from "../Generic/Calendar/Calendar";
import Selector from "../Generic/Select/Selector";
import { Slider } from "@mui/material";

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
      <div className={styles.expander}>
        <div className={styles.expanderHeader} onClick={handleExpand}>
          <div className={styles.expanderTitle}>Filters</div>
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
          </div>
        </div>
      </div>
    </div>
  );
};

export default Filters;
