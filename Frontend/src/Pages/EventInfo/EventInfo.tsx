import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import testImage from "../../assets/main.png";
import styles from "./EventInfo.module.scss";
import { EventItemExtended } from "../../lib/DTOs/Event";
import {
  Category,
  Payments,
  Place,
  CalendarMonth,
  AccessTime,
} from "@mui/icons-material";
import Price from "../../Components/Generic/Price/Price";
import { BlueButton } from "../../Components/Generic/Button/Buttons";

const EventInfo = () => {
  const { eventId } = useParams();
  const [currentEvent, setCurrentEvent] = useState<EventItemExtended | null>(
    null
  );
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);
  useEffect(() => {
    // get eventInfo
  }, [eventId]);

  return (
    <>
      <div className={styles.title}>
        <h3>Syberry .NET Hackaton</h3>
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
                Concert
              </li>
              <li>
                <Place
                  style={{ marginRight: 10 }}
                  color="primary"
                  fontSize="large"
                />
                Minsk
              </li>
              <li>
                <CalendarMonth
                  style={{ marginRight: 10 }}
                  color="primary"
                  fontSize="large"
                />
                {currentDate.toLocaleDateString()}
              </li>
              <li>
                <AccessTime
                  style={{ marginRight: 10 }}
                  color="primary"
                  fontSize="large"
                />
                {currentDate.toLocaleTimeString()}
              </li>
              <li>
                <Payments
                  style={{ marginRight: 17 }}
                  color="primary"
                  fontSize="large"
                />
                <Price value={10} />
              </li>
            </ul>
          </div>
          <div className={styles.buyBlock}>
          <h5>Buy now! Tickets left: 5</h5>
          <BlueButton text="Buy ticket" onClick={() => console.log()} />
        </div>
        </div>

        <div className={styles.description}>
          <p>
            Lorem Ipsum is simply dummy text of the printing and typesetting
            industry. Lorem Ipsum has been the industry's standard dummy text
            ever since the 1500s, when an unknown printer took a galley of type
            and scrambled it to make a type specimen book. It has survived not
            only five centuries, but also the leap into electronic typesetting,
            remaining essentially unchanged. It was popularised in the 1960s
            with the release of Letraset sheets containing Lorem Ipsum passages,
            and more recently with desktop publishing software like Aldus
            PageMaker including versions of Lorem Ipsum.
          </p>
        </div>
      </div>
    </>
  );
};

export default EventInfo;
