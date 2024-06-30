import styles from "./Buttons.module.scss";

type Props = {
  text: string;
  onClick?: () => void;
  type?: "submit" | "reset" | "button" | undefined;
  isDisabled?: boolean;
};

const BlueButton = ({
  text,
  onClick = () => {},
  type = "button",
  isDisabled = false,
}: Props) => {
  return (
    <button
      type={type}
      className={`${styles.btn} ${styles.blueButton}`}
      onClick={onClick}
      disabled={isDisabled}
    >
      {text}
    </button>
  );
};

const WhiteButton = ({
  text,
  onClick = () => {},
  type = "button",
  isDisabled = false,
}: Props) => {
  return (
    <button
      type={type}
      className={`${styles.btn} ${styles.whiteButton}`}
      onClick={onClick}
      disabled={isDisabled}
    >
      {text}
    </button>
  );
};

export { BlueButton, WhiteButton };
