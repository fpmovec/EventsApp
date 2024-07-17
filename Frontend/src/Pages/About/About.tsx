import styles from "./About.module.scss";

const About = () => {
  return (
    <div className={styles.main}>
      <h4
        style={{
          fontWeight: 400,
          letterSpacing: 2,
          textAlign: "center",
          marginBottom: 45,
        }}
      >
        Your Gateway to Unforgettable Experiences! 🎟️
      </h4>
      <div className={styles.description}>
        <p>
          Are you ready to immerse yourself in the world of entertainment,
          culture, and excitement? Whether you’re a music enthusiast, a social
          butterfly, or an adventure seeker, we’ve got something special just
          for you.
          <br />
          <br />
          🌟 Why Choose Us?
        </p>
        <ul>
          <li>
            <span>Convenient booking.</span> Thanks to our intuitive interface, booking
            tickets becomes easier than ever. Just find your favorite events,
            select the right places and secure them for yourself. From exciting
            concerts to thought-provoking meetings and vibrant festivals, we
            organize a wide range of events.
          </li>
          <li>
            <span>Security.</span> Your privacy is important to us. We use the most advanced
            encryption methods to protect your data. 🎉
          </li>
          <li>
            <span>Round-the-clock support.</span> Any questions? Our friendly support team is
            ready to help you at any time. Join us community today
            and let the magic happen! 🎉
          </li>
        </ul>
        <p>
          Whether it's front row seats at a rock concert or a cozy corner at a
          literary festival, we guarantee that your ticket to happiness
          will be just a few steps away. 🎫✨
        </p>
      </div>
      <br/>
    </div>
  );
};

export default About;
