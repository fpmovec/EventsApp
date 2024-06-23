import Footer from "./Components/Footer/Footer";
import Header from "./Components/Header/Header";
import RouterComponent from "./Components/Router/RouterComponent";
import { BrowserRouter } from "react-router-dom";

function App() {
  return (
    <BrowserRouter>
      <Header />
      <RouterComponent />
      <Footer />
    </BrowserRouter>
  );
}

export default App;
