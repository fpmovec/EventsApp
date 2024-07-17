import { useNavigate } from "react-router-dom";
import { EventItem } from "../../lib/Models/Event";
import styles from "./EventItem.module.scss";
import { BlueButton } from "../Generic/Button/Buttons";
import { Edit, Delete } from "@mui/icons-material";
import Price from "../Generic/Price/Price";
import { Place, CalendarMonth } from "@mui/icons-material";

type Props = {
  eventItem: EventItem;
  isExtendedFunctionality?: boolean;
  onDelete: (eventId: string) => void;
};

const EventBrief = ({ eventItem, onDelete, isExtendedFunctionality = false}: Props) => {
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);
  const navigate = useNavigate();

  return (
    <div className={styles.item}>
      <div className={styles.info}>
        <div className={styles.eventImage}>
          <img src={`https://localhost:7107/${eventItem.image.path}`} alt="No Image" />
        </div>
        <div className={styles.mainInfo}>
          <div className={styles.title}>
            <h4>{eventItem.name}</h4>
            <div className={styles.category}>{eventItem.category.name}</div>
          </div>
          <p>{eventItem.briefDescription}</p>
        </div>
        <div className={styles.buttonBlock}>
          <BlueButton
            text="More..."
            onClick={() => navigate(`/event/${eventItem.id}`)}
          />
          {isExtendedFunctionality && (
            <>
              <div className={styles.extendedButtons}>
                <div onClick={() => navigate(`/event/edit/${eventItem.id}`)}>
                  <Edit color="primary" />
                </div>
                <div onClick={() => onDelete(eventItem.id)}>
                  <Delete color="primary" />
                </div>
              </div>
            </>
          )}
        </div>
      </div>
      <div className={styles.additionalInfo}>
        <div>
          <Place color="primary" />
          <span>{eventItem.place}</span>
        </div>
        <div>
          <CalendarMonth color="primary" />
          <span>{new Date(eventItem.date).toLocaleDateString()}</span>
        </div>
        <Price value={eventItem.price} />
      </div>
    </div>
  );
};

export default EventBrief;
