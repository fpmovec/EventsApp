import EventBrief from '../../Components/EventItem/EventItem';
import Filters from '../../Components/Filters/Filters';
import styles from './AllEvents.module.scss'

const AllEvents = () => {

    return <div>
        <div className={styles.filyters}>
            <Filters />
        </div>
        <div className={styles.eventsList}>
            <EventBrief />
            <EventBrief />
            <EventBrief />
        </div>
    </div>
}

export default AllEvents;