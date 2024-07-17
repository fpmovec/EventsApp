import { baseUrl } from "../../Constants";
import { Booking } from "../../Models/Booking";

export const GetParticipantBookings = async (
  userId: string,
  token: string
): Promise<Booking[]> => {
  const response = await fetch(
    `${baseUrl}/api/booking/get-by-participant/${userId}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + `${token}`,
      },
      credentials: "omit",
    }
  );

  const bookings = (await response.json()) as Booking[];

  return bookings;
};
