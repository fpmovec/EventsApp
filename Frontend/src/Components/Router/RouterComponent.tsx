import { Route, Routes } from "react-router-dom";
import EventInfo from "../../Pages/EventInfo/EventInfo";
import HomePage from "../../Pages/Home/Home";
import AllEvents from "../../Pages/AllEvents/AllEvents";
import RegisterLoginPage from "../../Pages/RegisterLogin/RegisterLoginPage";
import Profile from "../../Pages/Profile/Profile";
import MyTickets from "../../Pages/MyTickets/Mytickets";
import About from "../../Pages/About/About";
import Booking from "../../Pages/Booking/Booking";

const RouterComponent = () => {
  return (
    <Routes>
      <Route path="/event/:eventId" element={<EventInfo />} />
      <Route path="/events" element={<AllEvents />} />
      <Route path="/login" element={<RegisterLoginPage />} />
      <Route path="/profile" element={<Profile />} />
      <Route path="/booked" element={<MyTickets />}/>
      <Route path="/about" element={<About />} />
      <Route path="/booking/:eventId" element={<Booking />}/>
      <Route path="/" element={<HomePage />} />
      <Route path="*" element={<HomePage />} />
    </Routes>
  );
};

export default RouterComponent;
