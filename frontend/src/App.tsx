import { FlexboxGrid } from "rsuite";
import { ContainerBlock } from "./components/Container";
import { generateOauthLink } from "./utils/oauth";
import { useCookies } from "react-cookie";

export default function App() {
	const [cookie] = useCookies(["token"]);

	if (cookie.token) return <ContainerBlock />;
	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 15, textAlign: "center" }}>
				<h3>
					<a href={generateOauthLink()}>Login with Discord</a>
				</h3>
			</FlexboxGrid>
		</>
	);
}
