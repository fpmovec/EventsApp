import styles from './Price.module.scss'

const Price = ({value}: {value: number}) => {
    return <div className={styles.price}>{value}</div>
}

export default Price;