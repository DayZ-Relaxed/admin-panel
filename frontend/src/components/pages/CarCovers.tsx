import { useEffect, useState } from "react";
import Loader from "../misc/Loader";
import { Button, Col, FlexboxGrid, Input, Pagination, Row, Table } from "rsuite";
import { CarCovers } from "../../types/CarCovers";
import { Player } from "../../types/Player";
import { AutoComplete } from "rsuite";

const { Column, HeaderCell, Cell } = Table;

const STYLE = { paddingRight: 100 };

export default function CarCoversPage({ mapId }: any) {
	const [limit, setLimit] = useState(100);
	const [page, setPage] = useState(1);

	const [carcovers, setCarcovers] = useState<CarCovers[]>([]);
	const [players, setPlayers] = useState<string[]>([]);
	const [loading, setLoading] = useState<boolean>(false);

	const [playerName, setPlayerName] = useState<string | undefined>(undefined);
	const [daysAgo, setDaysAgo] = useState<string>("1");
	const [posX, setPosX] = useState<string | undefined>(undefined);
	const [posY, setPosY] = useState<string | undefined>(undefined);
	const [posZ, setPosZ] = useState<string | undefined>(undefined);

	function search() {
		setLoading(true);
		setPage(1);

		let url = `${process.env.REACT_APP_API_URL}/cars/${mapId}/covers?daysAgo=${daysAgo}`;
		if (playerName !== undefined) url += `&playerName=${playerName}`;
		if (posX !== undefined) url += `&posX=${posX}`;
		if (posY !== undefined) url += `&posY=${posY}`;
		if (posZ !== undefined) url += `&posZ=${posZ}`;

		fetch(url, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				setCarcovers(res);
				setLoading(false);
			})
			.catch(err => {
				console.error(err);
				setLoading(false);
			});
	}

	const handleChangeLimit = (dataKey: any) => {
		setPage(1);
		setLimit(dataKey);
	};
	const data = carcovers.filter((v, i) => {
		const start = limit * (page - 1);
		const end = start + limit;
		return i >= start && i < end;
	});

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/players/${mapId}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => setPlayers(res.map((player: Player) => player.playerName)));
	}, []);

	return (
		<>
			<FlexboxGrid justify="center">
				<FlexboxGrid.Item colspan={4} style={STYLE}>
					<label>Player Name</label>
					<AutoComplete
						data={players}
						value={playerName ?? ""}
						onChange={value => (value === "" ? setPlayerName(undefined) : setPlayerName(value))}
					/>
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={4} style={STYLE}>
					<label>Days Ago</label>
					<Input value={daysAgo} defaultValue={"1"} onChange={value => (value === "" ? setDaysAgo("5") : setDaysAgo(value))} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={4} style={STYLE}>
					<label>X Coordinate</label>
					<Input value={posX ?? ""} onChange={value => (value === "" ? setPosX(undefined) : setPosX(value))} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={4} style={STYLE}>
					<label>Z Coordinate</label>
					<Input value={posZ ?? ""} onChange={value => (value === "" ? setPosZ(undefined) : setPosZ(value))} />
				</FlexboxGrid.Item>
				<FlexboxGrid.Item colspan={4} style={STYLE}>
					<label>Y Coordinate</label>
					<Input value={posY ?? ""} onChange={value => (value === "" ? setPosY(undefined) : setPosY(value))} />
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
							setPosY(undefined);
							setPosZ(undefined);
							setPlayerName(undefined);
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
							<Table height={data.length * 46 + 100} width={1400} data={data} hover={true}>
								<Column width={200}>
									<HeaderCell>Date</HeaderCell>
									<Cell dataKey="date" />
								</Column>

								<Column width={200}>
									<HeaderCell>Player Name</HeaderCell>
									<Cell dataKey="playerName" />
								</Column>

								<Column width={200}>
									<HeaderCell>Steam ID</HeaderCell>
									<Cell dataKey="steamId" />
								</Column>

								<Column width={100}>
									<HeaderCell>Action</HeaderCell>
									<Cell dataKey="action" />
								</Column>

								<Column width={200}>
									<HeaderCell>Car</HeaderCell>
									<Cell dataKey="car" />
								</Column>

								<Column width={100}>
									<HeaderCell>Pos X</HeaderCell>
									<Cell dataKey="posX" />
								</Column>

								<Column width={100}>
									<HeaderCell>Pos Y</HeaderCell>
									<Cell dataKey="posY" />
								</Column>

								<Column width={150}>
									<HeaderCell>Pos Z</HeaderCell>
									<Cell dataKey="posZ" />
								</Column>

								<Column width={150} fixed="right">
									<HeaderCell>...</HeaderCell>

									<Cell style={{ padding: "3px" }}>
										{row => (
											<Button
												appearance="link"
												onClick={() => {
													setPosX(row.posX);
													setPosY(row.posY);
													setPosZ(row.posZ);
													setPlayerName(undefined);
													window.scrollTo(0, 0);
												}}
											>
												Replace Coordinates
											</Button>
										)}
									</Cell>
								</Column>
							</Table>
							<div style={{ padding: 20 }}>
								<Pagination
									prev
									next
									first
									last
									ellipsis
									boundaryLinks
									maxButtons={5}
									size="xs"
									layout={["total", "-", "limit", "|", "pager", "skip"]}
									total={carcovers.length}
									limitOptions={[10, 50, 100, 150, 200]}
									limit={limit}
									activePage={page}
									onChangePage={setPage}
									onChangeLimit={handleChangeLimit}
								/>
							</div>
						</FlexboxGrid.Item>
					</FlexboxGrid>
				</>
			)}
		</>
	);
}
