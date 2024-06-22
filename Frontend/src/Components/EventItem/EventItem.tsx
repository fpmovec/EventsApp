import { useNavigate } from "react-router-dom";
import { EventItem } from "../../lib/DTOs/Event";
import styles from "./EventItem.module.scss";
import noImage from "../../assets/main.png";
import { BlueButton } from "../Generic/Button/Buttons";
import Price from "../Generic/Price/Price";

type Props = {
  eventItem: EventItem;
};

const EventBrief = () => {
  const currentDate = new Date();
  currentDate.setSeconds(0, 0);

  return (
    <div className={styles.item}>
      <div className={styles.info}>
        <div className={styles.eventImage}>
          <img src={noImage} alt="No Image" />
        </div>
        <div className={styles.mainInfo}>
          <div className={styles.title}>
            <h4>Any test name</h4>
            <div className={styles.category}>Concert</div>
          </div>
          <p>
            dkbdfkhdzbfdbgsdbvs,nbdbgbgdbg,nzdbghsgbhdsbgdbdbhdsbsghbgsdgzfgdsgsdhthtdfhdfhtdhdfhfhhtjjdjjtjdsjvdskdsbkbxvksdhfsdbjcbdjkhseufbsdvsdbsejkbesjkgf
          </p>
        </div>
        <div className={styles.buttonBlock}>
          <BlueButton
            text="More..."
            onClick={() => console.log("Hello, I'm the custom event!")}
          />
        </div>
      </div>
      <div className={styles.additionalInfo}>
        <div>Minsk</div>
        <div>{currentDate.toLocaleString()}</div>
        <Price value={10} />
      </div>
    </div>
  );
};

export default EventBrief;
