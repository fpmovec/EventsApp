const baseUrl = "https://localhost:7107";

export const DeleteEvent = async (
  eventId: string,
  token: string
): Promise<void> => {
  await fetch(`${baseUrl}/events/delete/${eventId}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
  });
};
