import { FlexboxGrid } from "rsuite";
import { ContainerBlock } from "./components/Container";
import { generateOauthLink } from "./utils/oauth";
import { useCookies } from "react-cookie";

export default function App() {
	const [cookie] = useCookies(["token"]);

	if (cookie.token) return <ContainerBlock />;
	return (
		<FlexboxGrid justify="center" style={{ marginTop: 15 }}>
			<FlexboxGrid.Item colspan={4}>
				<h3>
					<a href={generateOauthLink()}>Login in with Discord</a>
				</h3>
			</FlexboxGrid.Item>
		</FlexboxGrid>
	);
}
