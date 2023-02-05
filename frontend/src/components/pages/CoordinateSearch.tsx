import { useEffect, useState } from "react";
import Loader from "../misc/Loader";
import { Button, Col, FlexboxGrid, Input, Pagination, Row, Table } from "rsuite";
import { CarCovers } from "../../types/CarCovers";
import { CoordinateSearch } from "../../types/CoordinateSearch";

const { Column, HeaderCell, Cell } = Table;

const STYLE = { paddingRight: 100 };

export default function CoordinateSearchPage({ mapId }: any) {
	const [daysAgo, setDaysAgo] = useState<string>("1");
	const [radius, setRadius] = useState<string>("60");
	const [posX, setPosX] = useState<string | undefined>(undefined);
	const [posZ, setPosZ] = useState<string | undefined>(undefined);

	const [coordinateSearch, setCoordinateSearch] = useState<CoordinateSearch[]>([]);
	const [loading, setLoading] = useState<boolean>(false);

	function search() {
		setLoading(true);

		let url = `${process.env.REACT_APP_API_URL}/mappositions/${mapId}?daysAgo=${daysAgo}&radius=${radius}`;
		if (posX !== undefined) url += `&posX=${posX}`;
		if (posZ !== undefined) url += `&posZ=${posZ}`;

		fetch(url, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				setCoordinateSearch(res);
				setLoading(false);
			})
			.catch(err => {
				console.error(err);
				setLoading(false);
			});
	}

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

			{loading && <Loader />}
			{!loading && (
				<>
					<FlexboxGrid justify="center" style={{ marginTop: 25 }}>
						<FlexboxGrid.Item colspan={20}>
							<Table height={coordinateSearch.length * 46 + 100} width={750} data={coordinateSearch} hover={true}>
								<Column width={200}>
									<HeaderCell>Date</HeaderCell>
									<Cell dataKey="date" />
								</Column>

								<Column width={200}>
									<HeaderCell>Player Name</HeaderCell>
									<Cell dataKey="playerName" />
								</Column>

								<Column width={100}>
									<HeaderCell>Pos X</HeaderCell>
									<Cell dataKey="posX" />
								</Column>

								<Column width={100}>
									<HeaderCell>Pos Y</HeaderCell>
									<Cell dataKey="posZ" />
								</Column>

								<Column width={150}>
									<HeaderCell>Pos Z</HeaderCell>
									<Cell dataKey="posY" />
								</Column>
							</Table>
						</FlexboxGrid.Item>
					</FlexboxGrid>
				</>
			)}
		</>
	);
}
