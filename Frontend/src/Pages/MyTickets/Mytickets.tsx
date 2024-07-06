import { useEffect, useState } from "react";
import { useAppSelector } from "../../lib/Redux/Hooks";
import styles from "./MyTickets.module.scss";
import { Booking } from "../../lib/Models/Booking";
import { GetParticipantBookings } from "../../lib/Requests/GET/Bookings";
import { CancelBooking } from "../../lib/Requests/DELETE/Booking";
import BookingItem from "../../Components/BookingItem/BookingItem";

const MyTickets = () => {
  const token = useAppSelector((state) => state.auth.tokens.mainToken);
  const currentUser = useAppSelector((state) => state.auth.user);

  const [bookedTickets, setBookedTickets] = useState<Booking[]>([]);

  const getBookings = async () => {
    const bookings = await GetParticipantBookings(
      currentUser?.id as string,
      token
    );

    setBookedTickets(bookings);
  };

  useEffect(() => {
    getBookings();
  }, []);

  const cancelBooking = (id: number) => {
    const cancel = async () => {
      await CancelBooking(id, currentUser?.id as string, token);
    };

    cancel();
    getBookings();
  };

  return (
    <>
      <div className={styles.title}>
        <h3>Booked tickets</h3>
      </div>
      {bookedTickets.length > 0 ? (
        <div className={styles.bookingsList}>
          {bookedTickets.map((b, i) => (
            <BookingItem key={i} item={b} onDelete={cancelBooking} />
          ))}
        </div>
      ) : (
        <>
          <h4
            style={{
              fontWeight: 300,
              letterSpacing: 2,
              textAlign: "center",
              marginTop: 35,
              marginBottom: 400,
            }}
          >
            No booked tickets yet
          </h4>
        </>
      )}
    </>
  );
};

export default MyTickets;
