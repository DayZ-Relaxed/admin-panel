import { BrowserRouter, Route, Routes } from "react-router-dom";
import ReactDOM from "react-dom/client";
import { CustomProvider } from "rsuite";
import "rsuite/dist/rsuite.min.css";

import "./index.css";
import App from "./App";
import Oauth from "./routes/oauth";

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement);
root.render(
	<CustomProvider theme="dark">
		<BrowserRouter>
			<Routes>
				<Route path="/" element={<App />} />
				<Route path="oauth" element={<Oauth />} />
			</Routes>
		</BrowserRouter>
	</CustomProvider>,
);
