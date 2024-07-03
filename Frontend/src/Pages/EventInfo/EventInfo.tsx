import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import testImage from "../../assets/main.png";
import styles from "./EventInfo.module.scss";
import {
  Category,
  Payments,
  Place,
  CalendarMonth,
  AccessTime,
} from "@mui/icons-material";
import Price from "../../Components/Generic/Price/Price";
import { BlueButton } from "../../Components/Generic/Button/Buttons";
import { EventItemExtended } from "../../lib/DTOs/Event";
import { GetEventById } from "../../lib/Requests/GET/EventsRequests";
import { IsAuthenticated } from "../../lib/Requests/GET/Auth";
import { useAppSelector } from "../../lib/Redux/Hooks";

const EventInfo = () => {
  const { eventId } = useParams();
  const navigate = useNavigate();
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  const [currentEvent, setCurrentEvent] = useState<EventItemExtended | null>(
    null
  );
  useEffect(() => {
    const getEvent = async () => {
      const event = await GetEventById(eventId as string);
      setCurrentEvent(event);
    };
    getEvent();
  }, [eventId]);

  return (
    <>
      {currentEvent === null ? (
        <></>
      ) : (
        <>
          <div className={styles.title}>
            <h3>{currentEvent?.name}</h3>
          </div>
          <div className={styles.info}>
            <div className={styles.mainInfo}>
              <div className={styles.image}>
                <img src={testImage} alt="Event image" />
              </div>
              <div style={{ marginLeft: 50 }}>
                <ul className={styles.list}>
                  <li>
                    <Category
                      style={{ marginRight: 10 }}
                      color="primary"
                      fontSize="large"
                    />
                    {currentEvent?.category.name}
                  </li>
                  <li>
                    <Place
                      style={{ marginRight: 10 }}
                      color="primary"
                      fontSize="large"
                    />
                    {currentEvent?.place}
                  </li>
                  <li>
                    <CalendarMonth
                      style={{ marginRight: 10 }}
                      color="primary"
                      fontSize="large"
                    />
                    {new Date(currentEvent!.date).toLocaleDateString()}
                  </li>
                  <li>
                    <AccessTime
                      style={{ marginRight: 10 }}
                      color="primary"
                      fontSize="large"
                    />
                    {new Date(currentEvent!.date).toLocaleTimeString()}
                  </li>
                  <li>
                    <Payments
                      style={{ marginRight: 17 }}
                      color="primary"
                      fontSize="large"
                    />
                    <Price value={currentEvent?.price as number} />
                  </li>
                </ul>
              </div>
              <div className={styles.buyBlock}>
                <h5>
                  {currentEvent.isSoldOut ? (
                    <>Sold out</>
                  ) : (
                    <>
                      Buy now! Tickets left:{" "}
                      {currentEvent?.remainingTicketsCount}
                    </>
                  )}
                </h5>
                <BlueButton
                  text="Buy ticket"
                  onClick={() =>
                    isAuthenticated
                      ? navigate(`/booking/${eventId}`)
                      : navigate(`/login`)
                  }
                  isDisabled={currentEvent.isSoldOut}
                />
              </div>
            </div>

            <div className={styles.description}>
              <p>{currentEvent?.description}</p>
            </div>
          </div>
        </>
      )}
    </>
  );
};

export default EventInfo;
