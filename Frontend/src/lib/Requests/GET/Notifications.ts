import { Refresh } from "@mui/icons-material";
import { DetailsChangedEvent } from "../../Models/Notifications";
import { AuthResult } from "../../Models/AuthResponse";

const baseUrl = "https://localhost:7107";

export const GetNotificationsByUserId = async (
  token: string,
  userId: string
): Promise<DetailsChangedEvent[]> => {
  const response = await fetch(`${baseUrl}/api/notifications/get/${userId}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: "Bearer " + `${token}`,
    },
    credentials: "omit",
  });

  const notifications: DetailsChangedEvent[] = await response.json();

  return notifications;
};
