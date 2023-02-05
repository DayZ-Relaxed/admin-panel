import { Navbar, Nav, Avatar } from "rsuite";
import { CSSProperties, useEffect, useState } from "react";
import { NavBarComponent } from "../../types/components/Navbar";
import { getAvatarUrl } from "../../utils/helpers";
import { useCookies } from "react-cookie";

const headerStyles: CSSProperties = {
	fontSize: 16,
	height: 56,
	background: "#1a1d24",
	color: " #fff",
	overflow: "hidden",
	fontWeight: "bold",
};

export default function NavBar({ user, updateMap }: NavBarComponent) {
	const [cookie, setCookie] = useCookies(["server"]);
	const [currentServer, setCurrentServer] = useState(cookie.server ?? "Deer Isle");

	useEffect(() => {
		setCookie("server", currentServer ?? "Deer Isle");
		updateMap(currentServer ?? "Deer Isle");
	}, [currentServer]);

	return (
		<Navbar>
			<Navbar.Brand>
				<div style={headerStyles}>DayZ Relaxed Admin Panel</div>
			</Navbar.Brand>

			<Nav style={{ marginLeft: 20 }}>
				<Nav.Menu title={`Current Server: ${currentServer}`}>
					<Nav.Item eventKey="Deer Isle" onSelect={e => setCurrentServer(e)}>
						Deer Isle
					</Nav.Item>
					<Nav.Item eventKey="Chernarus" onSelect={e => setCurrentServer(e)}>
						Chernarus
					</Nav.Item>
				</Nav.Menu>
			</Nav>
			<Nav pullRight>
				<Nav.Item>
					{user ? (
						<Avatar size="md" circle src={getAvatarUrl(user.avatar, user.id)} alt={`${user.username}#${user.discriminator} avatar`} />
					) : (
						<Avatar size="md" circle />
					)}
				</Nav.Item>
			</Nav>
		</Navbar>
	);
}
