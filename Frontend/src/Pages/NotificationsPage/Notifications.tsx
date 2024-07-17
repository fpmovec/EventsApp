import { useEffect, useState } from "react";
import styles from "./Notifications.module.scss";
import { DetailsChangedEvent } from "../../lib/Models/Notifications";
import { useAppSelector } from "../../lib/Redux/Hooks";
import { GetNotificationsByUserId } from "../../lib/Requests/GET/Notifications";
import NotificationItem from "../../Components/NotificationItem/NotificationItem";

const NotificationsPage = () => {
  const [notifications, setNotifications] = useState<DetailsChangedEvent[]>([]);
  const token = useAppSelector((state) => state.auth.tokens).mainToken;
  const userId = useAppSelector((state) => state.auth.user)?.id;

  useEffect(() => {
    GetNotificationsByUserId(token, userId as string).then((res) => {
      setNotifications(res as DetailsChangedEvent[]);
    });
  }, [notifications.length, userId]);

  return (
    <>
      <div className={styles.title}>
        <h3>Notifications</h3>
      </div>
      <div className={styles.notificationsList}>
        {notifications.length > 0 ? (
          <>
            {notifications.map((n, i) => (
              <NotificationItem item={n} key={i} />
            ))}
          </>
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
              No notifications yet
            </h4>
          </>
        )}
      </div>
    </>
  );
};

export default NotificationsPage;
