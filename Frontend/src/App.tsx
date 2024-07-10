import Footer from "./Components/Footer/Footer";
import Header from "./Components/Header/Header";
import RouterComponent from "./Components/Router/RouterComponent";
import { BrowserRouter } from "react-router-dom";
import { Provider } from "react-redux";
import store from "./lib/Redux/Store";
import NotificationsClient from "./lib/SignalR/NotificationsClient";

function App() {
  return (
    <BrowserRouter>
      <Provider store={store}>
        <NotificationsClient />
        <Header />
        <RouterComponent />
        <Footer />
      </Provider>
    </BrowserRouter>
  );
}

export default App;
