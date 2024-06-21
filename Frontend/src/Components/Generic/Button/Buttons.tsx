import styles from './Buttons.module.scss'

type Props = {
    text: string;
    onClick: () => void;
}

const BlueButton = ({ text, onClick }: Props) => {
   return <button className={`${styles.btn} ${styles.blueButton}`} onClick={onClick}>{text}</button>
}

const WhiteButton = ({ text, onClick }: Props) => {
    return <button className={`${styles.btn} ${styles.whiteButton}`} onClick={onClick}>{text}</button>
}

export {
    BlueButton,
    WhiteButton
};