import { Route, Routes } from "react-router-dom";
import EventInfo from "../../Pages/EventInfo/EventInfo";
import HomePage from "../../Pages/Home/Home";
import AllEvents from "../../Pages/AllEvents/AllEvents";

const RouterComponent = () => {
  return (
    <Routes>
      <Route path="/event/:eventId" element={<EventInfo />} />
      <Route path="/events" element={<AllEvents />} />
      <Route path="/" element={<HomePage />} />
      <Route path="*" element={<HomePage />} />
    </Routes>
  );
};

export default RouterComponent;
