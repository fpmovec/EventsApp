const baseUrl = "https://localhost:7107";

export const CancelBooking = async (
  bookingId: number,
  userId: string,
  token: string
): Promise<void> => {
  await fetch(`${baseUrl}/api/booking/cancel/${bookingId}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
    body: JSON.stringify(userId),
  });
};
