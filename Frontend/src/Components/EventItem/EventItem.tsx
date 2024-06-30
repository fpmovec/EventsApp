import { useNavigate } from "react-router-dom";
import { EventItem } from "../../lib/DTOs/Event";
import styles from "./EventItem.module.scss";
import noImage from "../../assets/main.png";
import { BlueButton } from "../Generic/Button/Buttons";
import Price from "../Generic/Price/Price";
import { Place, CalendarMonth } from "@mui/icons-material";

type Props = {
  eventItem: EventItem;
};

const EventBrief = ({eventItem}: Props) => {
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);
  const navigate = useNavigate();

  return (
    <div className={styles.item}>
      <div className={styles.info}>
        <div className={styles.eventImage}>
          <img src={noImage} alt="No Image" />
        </div>
        <div className={styles.mainInfo}>
          <div className={styles.title}>
            <h4>{eventItem.name}</h4>
            <div className={styles.category}>{eventItem.category.name}</div>
          </div>
          <p>
            {eventItem.briefDescription}
          </p>
        </div>
        <div className={styles.buttonBlock}>
          <BlueButton text="More..." onClick={() => navigate(`/event/${eventItem.id}`)} />
        </div>
      </div>
      <div className={styles.additionalInfo}>
        <div><Place color="primary"/><span>{eventItem.place}</span></div>
        <div><CalendarMonth color="primary"/><span>{new Date(eventItem.date).toLocaleString()}</span></div>
        <Price value={eventItem.price} />
      </div>
    </div>
  );
};

export default EventBrief;
