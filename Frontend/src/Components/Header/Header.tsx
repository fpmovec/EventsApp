import styles from "./Header.module.scss";
import HeaderLink from "./HeaderLinks/Link";
import Sign from "./SignIn/Sign";

const Header = () => {
  return (
    <>
      <div className={styles.header}>
        <div className={styles.links}>
          <HeaderLink text="Home" path="#" />
          <HeaderLink text="All events" path="#" />
          <HeaderLink text="My tickets" path="#" />
          <HeaderLink text="About" path="#" />
        </div>
            <Sign />

      </div>
    </>
  );
};

export default Header;
