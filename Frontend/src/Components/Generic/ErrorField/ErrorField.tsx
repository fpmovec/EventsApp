import styles from "./ErrorField.module.css";

interface Props {
  data: string;
}

export const ErrorField = ({ data }: Props) =>
  data === null ? (
    <div className={styles.error}>Error</div>
  ) : (
    <div className={styles.error}>{data}</div>
  );