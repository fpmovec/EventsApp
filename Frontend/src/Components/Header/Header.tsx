import styles from "./Header.module.scss";
import HeaderLink from "./HeaderLinks/Link";
import Sign from "./SignIn/Sign";
import { useAppSelector } from "../../lib/Redux/Hooks";

const Header = () => {
  const isAuthenticated = useAppSelector((state) => state.auth.isAuthenticated);
  return (
    <>
      <div className={styles.header}>
        <div className={styles.links}>
          <HeaderLink text="Home" path="/" />
          <HeaderLink text="All events" path="/events?currentPage=1" />
          <HeaderLink text="My tickets" path={isAuthenticated ? '/booked' : '/login'} />
          <HeaderLink text="About" path="/about" />
        </div>
        <Sign />
      </div>
    </>
  );
};

export default Header;
