import { useEffect, useState } from "react";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import Selector from "../../Components/Generic/Select/Selector";
import styles from "./Home.module.scss";
import { WhiteButton } from "../../Components/Generic/Button/Buttons";
import EventBrief from "../../Components/EventItem/EventItem";

const HomePage = () => {
  const [date, setDate] = useState<Date>(new Date());
  const [city, setCity] = useState<string>("");
  const [category, setCategory] = useState<string>("");

  useEffect(() => {
    //console.log(date);
  }, [date]);

  return (
    <>
      <div className={styles.eventsSelector}>
        <div>
          <h1 className={styles.mainTitle}>Enjoy in the best way!</h1>
          <h3 className={styles.subtitle}>
            Enjoy our services during your vacation at any time
          </h3>
        </div>
        <div className={styles.eventsSelectorBlock}>
          <div className={styles.eventsSelectorContent}>
            <Calendar valueDate={date} handleValue={setDate} />
            <Selector
              label="City"
              value={city}
              source={["Minsk", "Moscow", "Mogilev"]}
              handleValue={setCity}
            />
            <Selector
              label="Category"
              value={category}
              source={["Festival", "Concert", "Conference"]}
              handleValue={setCategory}
            />
            <WhiteButton text="Search" onClick={() => console.log()} />
          </div>
        </div>
      </div>
      <h3
        id="popular"
        style={{ fontWeight: 500, textAlign: "center", marginTop: 30 }}
      >
        Explore our the most popular events
      </h3>
      <div className={styles.popular}>
        <EventBrief />
        <EventBrief />
        <EventBrief />
        <EventBrief />
        <EventBrief />
      </div>
    </>
  );
};

export default HomePage;
