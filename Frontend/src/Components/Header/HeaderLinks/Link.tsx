import styles from "./Link.module.scss";
import { Link } from "react-router-dom";

interface Props {
  text: string;
  path: string;
}

const HeaderLink = ({ text, path }: Props) => {
  return (
    <>
      <div className={styles.wrapper}>
        <Link to={path}>
          <span>{text}</span>
        </Link>
      </div>
    </>
  );
};

export default HeaderLink;
