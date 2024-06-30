import styles from "./Sign.module.scss";
import HeaderLink from "../HeaderLinks/Link";

const Sign = () => {
  return (
    <div className={styles.block}>
      <HeaderLink text="Sign In" path="/login"/>
    </div>
  );
};

export default Sign;