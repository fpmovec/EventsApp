import { useEffect, useState } from "react";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import Selector from "../../Components/Generic/Select/Selector";
import styles from "./Home.module.scss";
import { WhiteButton } from "../../Components/Generic/Button/Buttons";
import EventBrief from "../../Components/EventItem/EventItem";
import { useNavigate } from "react-router-dom";
import { EventItem } from "../../lib/Models/Event";
import { GetPopularEvents } from "../../lib/Requests/GET/EventsRequests";
import { useAppDispath, useAppSelector } from "../../lib/Redux/Hooks";
import { GetAllCategories } from "../../lib/Requests/GET/Categories";
import { setCategoriesList } from "../../lib/Redux/Slices";

const HomePage = () => {
  const [date, setDate] = useState<Date>(new Date());
  const [city, setCity] = useState<string>("");
  const [category, setCategory] = useState<string>("");
  const [popularEvents, setPopularEvents] = useState<EventItem[]>([]);
  const [categories, setCategories] = useState<string[]>([]);

  const currentUser = useAppSelector((state) => state.auth.user);

  const navigate = useNavigate();
  const dispatch = useAppDispath();

  useEffect(() => {
    const getPopularEvents = async () => {
      const events = await GetPopularEvents();
      setPopularEvents(events);
    };

    const getCategories = async () => {
      const categories = await GetAllCategories();
      setCategories(categories);
      dispatch(setCategoriesList(categories))
    };

    getPopularEvents();
    getCategories();
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
              source={categories}
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
          <EventBrief
            eventItem={e}
            key={i}
            isExtendedFunctionality={
              currentUser === undefined || !currentUser.isAdmin ? false : true
            }
            onDelete={() => {}}
          />
        ))}
      </div>
    </>
  );
};

export default HomePage;
