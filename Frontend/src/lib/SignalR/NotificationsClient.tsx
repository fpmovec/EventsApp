import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect, useState } from "react";
import { DetailsChangedEvent } from "../Models/Notifications";
import { IconButton, Snackbar } from "@mui/material";
import { Close } from "@mui/icons-material";
import { useAppSelector } from "../Redux/Hooks";
import { baseUrl } from "../Constants";

const NotificationsClient = () => {
  const [connection, setConnection] = useState<HubConnection>();
  const [notification, setNotification] = useState<DetailsChangedEvent | null>(
    null
  );

  const [isOpen, setOpen] = useState<boolean>(false);
  const userId = useAppSelector((state) => state.auth.user)?.id;
  const handleClosePopup = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setOpen(false);
  };

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${baseUrl}/api/notification`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("Connected!");

          connection.on("ReceiveMessageAsync", (message) => {
            setOpen(true);
            setNotification(message as DetailsChangedEvent);
          });
        })
        .catch((e) => console.log("Connection failed", e));
    }
  }, [connection]);

  return (
    <>
      {isOpen && notification?.userIds.includes(userId as string) && (
        <Snackbar
          open={isOpen}
          autoHideDuration={10000}
          onClose={handleClosePopup}
          message={notification?.message}
          action={
            <IconButton
              size="small"
              aria-label="close"
              color="inherit"
              onClick={handleClosePopup}
            >
              <Close fontSize="small" />
            </IconButton>
          }
        />
      )}
    </>
  );
};

export default NotificationsClient;
