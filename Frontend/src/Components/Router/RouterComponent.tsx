import { Route, Routes } from "react-router-dom";
import EventInfo from "../../Pages/EventInfo/EventInfo";
import HomePage from "../../Pages/Home/Home";
import AllEvents from "../../Pages/AllEvents/AllEvents";
import RegisterLoginPage from "../../Pages/RegisterLogin/RegisterLoginPage";
import About from "../../Pages/About/About";
import Booking from "../../Pages/Booking/Booking";
import MyTickets from "../../Pages/MyTickets/MyTickets";
import CreateOrEditEventPage from "../../Pages/CreateOrEditEvent/CreateOrEditEventPage";
import NotificationsPage from "../../Pages/NotificationsPage/Notifications";

const RouterComponent = () => {
  return (
    <Routes>
      <Route path="/event/:eventId" element={<EventInfo />} />
      <Route path="/events" element={<AllEvents />} />
      <Route path="/login" element={<RegisterLoginPage />} />
      <Route path="/booked" element={<MyTickets />} />
      <Route path="/about" element={<About />} />
      <Route path="/booking/:eventId" element={<Booking />} />
      <Route path="/event/edit/:eventId?" element={<CreateOrEditEventPage />}/>
      <Route path="/notifications" element={<NotificationsPage />} />
      <Route path="/" element={<HomePage />} />
      <Route path="*" element={<HomePage />} />
    </Routes>
  );
};

export default RouterComponent;
