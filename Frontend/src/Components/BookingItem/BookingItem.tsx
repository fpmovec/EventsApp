import { useNavigate } from "react-router-dom";
import { Booking } from "../../lib/Models/Booking";
import styles from "./BookingItem.module.scss";
import { WhiteButton } from "../Generic/Button/Buttons";

type Props = {
  item: Booking;
  onDelete?: (id: number) => void;
};

const BookingItem = ({ item, onDelete = () => {} }: Props) => {
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);
  const navigate = useNavigate();

  const bookingDate = new Date(item.createdDate);
  bookingDate.setSeconds(0);

  return (
    <div className={styles.item}>
      <div
        className={styles.title}
        onClick={() => navigate(`/event/${item.eventId}`)}
      >
        <h4>
          {item.eventName === "" ? <>Test title</> : <>{item.eventName}</>}
        </h4>
      </div>
      <div className={styles.info}>
        <ul>
          <li>
            <span>Booking ID: </span>
            {item.id}
          </li>
          <li>
            <span>Booking time: </span>
            {bookingDate.toLocaleString()}
          </li>
          <li>
            <span>Persons quantity: </span>
            {item.personsQuantity}
          </li>
        </ul>
        <ul>
          <li>
            <span>Full name: </span>
            {item.fullName}
          </li>
          <li>
            <span>Phone: </span>
            {item.phone}
          </li>
          <li>
            <span>Email: </span>
            {item.email}
          </li>
        </ul>
        <div>
          <div>
            <WhiteButton text="Cancel" onClick={() => onDelete(item.id)}/>
          </div>
        </div>
        <div className={styles.priceRow}>Total price: {item.totalPrice}$</div>
      </div>
    </div>
  );
};

export default BookingItem;
