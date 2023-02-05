import CloseOutlineIcon from "@rsuite/icons/CloseOutline";
import DashboardIcon from "@rsuite/icons/Dashboard";
import ExploreIcon from "@rsuite/icons/Explore";
import LineChartIcon from "@rsuite/icons/LineChart";
import RelatedMapIcon from "@rsuite/icons/RelatedMap";
import ScatterIcon from "@rsuite/icons/Scatter";
import GroupIcon from "@rsuite/icons/legacy/Group";
import { useEffect, useState } from "react";
import { useCookies } from "react-cookie";
import { Container, Content, Nav, Sidebar, Sidenav } from "rsuite";
import { DiscordUser } from "../types/Discord";
import { whatMapId } from "../utils/helpers";
import NavBar from "./misc/NavBar";
import CarCoversPage from "./pages/CarCovers";
import CoordinateSearchPage from "./pages/CoordinateSearch";
import DangerIcon from "@rsuite/icons/Danger";
import LogoutPage from "./pages/Logout";
import StatisticsPage from "./pages/Statistics";
import Territories from "./pages/Territories";
import TerritoriesMembersVisitsPage from "./pages/TerritoriesMembersVisitsPage";
import UsersPage from "./pages/UsersPage";
import VehicleDamagePage from "./pages/VehicleDamage";
import MoneyPage from "./pages/Money";
import CouponIcon from "@rsuite/icons/Coupon";

export const ContainerBlock = () => {
	const [activeKey, setActiveKey] = useState("1");
	const [comp, setComp] = useState<any>();
	const [_cookie, _setCookie, deleteCookie] = useCookies(["token"]);

	const [mapId, setMapId] = useState("0");
	const updateMapId = (value: any) => setMapId(whatMapId(value));

	const containers = {
		"1": <TerritoriesMembersVisitsPage mapId={mapId} />,
		"2": <Territories mapId={mapId} />,
		"3": <CarCoversPage mapId={mapId} />,
		"4": <VehicleDamagePage mapId={mapId} />,
		"5": <CoordinateSearchPage mapId={mapId} />,
		"6": <UsersPage mapId={mapId} />,
		"7": <StatisticsPage mapId={mapId} />,
		"8": <MoneyPage mapId={mapId} />,
		"9": <LogoutPage />,
	};

	useEffect(() => {
		// @ts-expect-error
		setComp(containers[activeKey]);
	}, [activeKey]);

	useEffect(() => {
		let tmp = activeKey;
		setActiveKey("0");
		new Promise(res => setTimeout(res, 100)).then(e => setActiveKey(tmp));
	}, [mapId]);

	const [user, setUser] = useState<DiscordUser | undefined>(undefined);
	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/discord/user`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => setUser(res))
			.catch(err => {
				console.error(err);
				deleteCookie("token");
			});
	}, []);

	return (
		<div>
			<NavBar user={user} updateMap={updateMapId} />
			<Container>
				<Sidebar style={{ display: "flex", flexDirection: "column" }} width={260}>
					<Sidenav>
						<Sidenav.Body>
							<Nav activeKey={activeKey} onSelect={setActiveKey}>
								<Nav.Item eventKey="1" icon={<ExploreIcon />}>
									Territories Visits
								</Nav.Item>
								<Nav.Item eventKey="2" icon={<DashboardIcon />}>
									Territories
								</Nav.Item>
								<Nav.Item eventKey="3" icon={<RelatedMapIcon />}>
									Cars
								</Nav.Item>
								<Nav.Item eventKey="4" icon={<DangerIcon />}>
									Vehicle Damage
								</Nav.Item>
								<Nav.Item eventKey="5" icon={<ScatterIcon />}>
									Coordinate Search
								</Nav.Item>
								<Nav.Item eventKey="6" icon={<GroupIcon />}>
									Players
								</Nav.Item>
								<Nav.Item eventKey="7" icon={<LineChartIcon />}>
									Statistics
								</Nav.Item>
								<Nav.Item eventKey="8" icon={<CouponIcon />}>
									Money
								</Nav.Item>
								<Nav.Item eventKey="9" icon={<CloseOutlineIcon />}>
									Logout
								</Nav.Item>
							</Nav>
						</Sidenav.Body>
					</Sidenav>
				</Sidebar>

				<Content style={{ marginTop: 12 }}>{comp}</Content>
			</Container>
		</div>
	);
};
