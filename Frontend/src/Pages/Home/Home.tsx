import { useEffect, useState } from "react";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import Selector from "../../Components/Generic/Select/Selector";
import styles from "./Home.module.scss";
import { WhiteButton } from "../../Components/Generic/Button/Buttons";
import EventBrief from "../../Components/EventItem/EventItem";
import { useNavigate } from "react-router-dom";
import { EventItem } from "../../lib/DTOs/Event";
import { GetPopularEvents } from "../../lib/Requests/GET/EventsRequests";

const HomePage = () => {
  const [date, setDate] = useState<Date>(new Date());
  const [city, setCity] = useState<string>("");
  const [category, setCategory] = useState<string>("");
  const [popularEvents, setPopularEvents] = useState<EventItem[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    const getPopularEvents = async () => {
      const events = await GetPopularEvents();
      setPopularEvents(events);
    };

    getPopularEvents();
  }, []);

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
              isRequired={true}
            />
            <Selector
              label="Category"
              value={category}
              source={["Festival", "Concert", "Conference"]}
              handleValue={setCategory}
              isRequired={true}
            />
            <WhiteButton
              text="Search"
              onClick={() =>
                navigate(
                  `/events?minDate=${date.toDateString()}&maxDate=${date.toDateString()}&category=${category}&place=${city}&currentPage=1`
                )
              }
            />
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
        {popularEvents.map((e, i) => (
          <EventBrief eventItem={e} key={i} />
        ))}
      </div>
    </>
  );
};

export default HomePage;

