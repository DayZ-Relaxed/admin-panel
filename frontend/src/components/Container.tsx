import { Container, Sidebar, Sidenav, Nav } from "rsuite";
import DashboardIcon from "@rsuite/icons/Dashboard";
import GroupIcon from "@rsuite/icons/legacy/Group";
import CloseOutlineIcon from "@rsuite/icons/CloseOutline";
import { useState } from "react";
import TerritoriesPage from "./pages/TerritoriesPage";
import UsersPage from "./pages/UsersPage";
import LogoutPage from "./pages/Logout";

const headerStyles = {
	padding: 18,
	fontSize: 16,
	height: 56,
	background: "#34c3ff",
	color: " #fff",
	overflow: "hidden",
};

const containers = {
	"1": <TerritoriesPage />,
	"2": <UsersPage />,
	"3": <LogoutPage />,
};

export const ContainerBlock = () => {
	const [activeKey, setActiveKey] = useState("1");
	// @ts-ignore
	const Content = containers[activeKey];

	return (
		<div>
			<Container>
				<Sidebar style={{ display: "flex", flexDirection: "column" }} width={260} collapsible>
					<Sidenav.Header>
						<div style={headerStyles}>
							<span style={{ marginLeft: 12 }}> DayZ Relaxed</span>
						</div>
					</Sidenav.Header>
					<Sidenav>
						<Sidenav.Body>
							<Nav activeKey={activeKey} onSelect={setActiveKey}>
								<Nav.Item eventKey="1" icon={<DashboardIcon />}>
									Territories
								</Nav.Item>
								<Nav.Item eventKey="2" icon={<GroupIcon />}>
									Players
								</Nav.Item>
								<Nav.Item eventKey="3" icon={<CloseOutlineIcon />}>
									Logout
								</Nav.Item>
							</Nav>
						</Sidenav.Body>
					</Sidenav>
				</Sidebar>

				<Container style={{ marginLeft: -100, marginTop: 12 }}>{Content}</Container>
			</Container>
		</div>
	);
};
