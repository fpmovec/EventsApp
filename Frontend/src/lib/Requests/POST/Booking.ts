import { BookingDTO } from "../../Models/Booking";

const baseUrl = "https://localhost:7107";

export const BookEvent = async (
  data: BookingDTO,
  token: string
): Promise<void> => {

  await fetch(`${baseUrl}/api/booking/book`, {
    method: "POST",
    headers: {
      Authorization: "Bearer " + `${token}`,
      "Content-Type": "application/json",
    },
    credentials: "omit",
    body: JSON.stringify(data),
  });
};
