import { baseUrl } from "../../Constants";

export const DeleteEvent = async (
  eventId: string,
  token: string
): Promise<void> => {
  await fetch(`${baseUrl}/api/events/delete/${eventId}`, {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
  });
};
