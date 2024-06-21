import styles from "./Link.module.scss";

interface Props {
  text: string;
  path: string;
}

const HeaderLink = ({ text, path }: Props) => {
  return (
    <>
      <div className={styles.wrapper}>
        <a href={path}>
          <span>{text}</span>
        </a>
      </div>
    </>
  );
};

export default HeaderLink;
