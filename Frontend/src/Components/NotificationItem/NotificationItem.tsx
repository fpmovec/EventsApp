import styles from './NotificationItem.module.scss'
import { DetailsChangedEvent } from '../../lib/Models/Notifications'


const NotificationItem = ({item} : {item: DetailsChangedEvent}) => {
    return <div className={styles.item}>
        <h6>{item.message}</h6>
    </div>
}

export default NotificationItem;