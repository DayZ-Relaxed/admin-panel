import { useEffect, useState } from "react";
import { Territory } from "../../types/Territory";
import Loader from "../misc/Loader";
import { Button, FlexboxGrid, Input, Table } from "rsuite";
import { whatMapId } from "../../utils/helpers";
import { VehicleDamage } from "../../types/VehicleDamage";

const { Column, HeaderCell, Cell } = Table;
const STYLE = { paddingRight: 100 };

export default function VehicleDamagePage({ mapId }: any) {
	const [isLoading, setIsLoading] = useState(true);
	const [data, setData] = useState<VehicleDamage[]>([]);

	const [daysAgo, setDaysAgo] = useState<string>("2");
	const [radius, setRadius] = useState<string | undefined>("60");
	const [posX, setPosX] = useState<string | undefined>(undefined);
	const [posZ, setPosZ] = useState<string | undefined>(undefined);

	function search() {
		setIsLoading(true);

		let url = `${process.env.REACT_APP_API_URL}/vehicledamage/${mapId}?daysAgo=${daysAgo}`;
		if (posX !== undefined) url += `&posX=${posX}`;
		if (posZ !== undefined) url += `&posZ=${posZ}`;
		if (radius !== undefined) url += `&radius=${radius}`;

		fetch(url, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				setData(res);
				setIsLoading(false);
			})
			.catch(err => {
				console.error(err);
				setIsLoading(false);
			});
	}

	useEffect(() => {
		search();
	}, []);

	return (
		<>
			<FlexboxGrid justify="center">
				<FlexboxGrid.Item colspan={5} style={STYLE}>
					<label>X Coordinate</label>
					<Input value={posX ?? ""} onChange={value => setPosX(value)} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={5} style={STYLE}>
					<label>Z Coordinate</label>
					<Input value={posZ ?? ""} onChange={value => setPosZ(value)} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={5} style={STYLE}>
					<label>Days Ago</label>
					<Input value={daysAgo} onChange={value => setDaysAgo(value)} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={5} style={STYLE}>
					<label>Radius</label>
					<Input value={radius} onChange={value => setRadius(value)} />
				</FlexboxGrid.Item>
			</FlexboxGrid>

			<FlexboxGrid justify="center" style={{ paddingTop: 10 }}>
				<FlexboxGrid.Item colspan={20} style={STYLE}>
					<Button appearance="primary" onClick={search}>
						Search
					</Button>
					<Button
						appearance="subtle"
						onClick={_e => {
							setPosX(undefined);
							setPosZ(undefined);
							window.scrollTo(0, 0);
						}}
						style={{ marginLeft: 20 }}
					>
						Clear
					</Button>
				</FlexboxGrid.Item>
			</FlexboxGrid>

			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={20}>
					<Table height={data.length * 46 + 100} width={1300} data={data} hover={true} loading={isLoading}>
						<Column width={200} align="center" fixed>
							<HeaderCell>Date</HeaderCell>
							<Cell dataKey="date" />
						</Column>

						<Column width={150}>
							<HeaderCell>Player Name</HeaderCell>
							<Cell dataKey="playerName" />
						</Column>

						<Column width={150}>
							<HeaderCell>Vehicle Name</HeaderCell>
							<Cell dataKey="vehicleName" />
						</Column>
						<Column width={150}>
							<HeaderCell>Weapon</HeaderCell>
							<Cell dataKey="weapon" />
						</Column>
						<Column width={200}>
							<HeaderCell>Ammo</HeaderCell>
							<Cell dataKey="ammo" />
						</Column>
						<Column width={150}>
							<HeaderCell>Zone</HeaderCell>
							<Cell dataKey="zone" />
						</Column>

						<Column width={100}>
							<HeaderCell>Pos X</HeaderCell>
							<Cell dataKey="posX" />
						</Column>
						<Column width={100}>
							<HeaderCell>Pos Y</HeaderCell>
							<Cell dataKey="posY" />
						</Column>
						<Column width={100}>
							<HeaderCell>Pos Z</HeaderCell>
							<Cell dataKey="posZ" />
						</Column>
					</Table>
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
