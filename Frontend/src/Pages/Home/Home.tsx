import React, { useEffect, useState } from "react";
import Calendar from "../../Components/Generic/Calendar/Calendar";
import Selector from "../../Components/Generic/Select/Selector";
import styles from './Home.module.scss';
import { BlueButton, WhiteButton } from "../../Components/Generic/Button/Buttons";

const HomePage = () => {
  const [date, setDate] = useState<Date>(new Date());
  const [transport, setTransport] = useState<string>("");
  
  useEffect(() => {
    console.log(date);
  }, [date]);

  return (
    <>
      <div className={styles.eventsSelector}>
        <div>
          <h1 className={styles.mainTitle}>Enjoy in the best way!</h1>
          <h3 className={styles.subtitle}>Enjoy our services during your vacation at any time</h3>
        </div>
        <div className={styles.eventsSelectorBlock}>
          <div className={styles.eventsSelectorContent}>
            <Calendar handleValue={setDate} />
            <Selector
              label="City"
              value={transport}
              source={["Minsk", "Moscow", "Mogilev"]}
              handleValue={setTransport}
            />
            <Selector
              label="City"
              value={transport}
              source={["Minsk", "Moscow", "Mogilev"]}
              handleValue={setTransport}
            />
            {/* <button className={styles.search}>
              <i className="fa fa-search fa-lg" aria-hidden={true}></i>
            </button> */}
            <WhiteButton text="Search" onClick={() => console.log()}/>
          </div>
        </div>
      </div>
    </>
  );
};

export default HomePage;
