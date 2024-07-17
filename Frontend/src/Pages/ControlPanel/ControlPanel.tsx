import { BlueButton } from "../../Components/Generic/Button/Buttons";
import styles from "./ControlPanel.module.scss";

const ControlPanel = () => {
  return (
    <>
      <div className={styles.title}>
        <h3>Control panel</h3>
      </div>
      <div className={styles.main}>
<div><BlueButton text="Create event"/></div>
<div></div>
<div></div>
<div></div>
<div></div>
      </div>
    </>
  );
};

export default ControlPanel;
