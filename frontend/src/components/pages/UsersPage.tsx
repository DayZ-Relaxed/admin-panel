import { FlexboxGrid, Table, Pagination } from "rsuite";
import { useEffect, useState } from "react";
import { Player } from "../../types/Player";
import Loader from "../misc/Loader";

const { Column, HeaderCell, Cell } = Table;

export default function UsersPage({ mapId }: any) {
	const [players, setPlayers] = useState<Player[]>([]);
	const [limit, setLimit] = useState(10);
	const [page, setPage] = useState(1);

	const handleChangeLimit = (dataKey: any) => {
		setPage(1);
		setLimit(dataKey);
	};

	const data = players.filter((v, i) => {
		const start = limit * (page - 1);
		const end = start + limit;
		return i >= start && i < end;
	});

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/players/${mapId}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => setPlayers(res));
	}, []);

	if (players.length === 0) return <Loader />;

	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 25 }}>
				<FlexboxGrid.Item colspan={5}>
					<h4>Players</h4>
				</FlexboxGrid.Item>
			</FlexboxGrid>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={17}>
					<Table height={data.length * 46 + 100} width={1000} data={data} hover={true}>
						<Column width={150} align="center" fixed>
							<HeaderCell>Player Id</HeaderCell>
							<Cell dataKey="playerId" />
						</Column>

						<Column width={150}>
							<HeaderCell>Name</HeaderCell>
							<Cell dataKey="playerName" />
						</Column>

						<Column width={400}>
							<HeaderCell>Country</HeaderCell>
							<Cell dataKey="country" />
						</Column>

						<Column width={300}>
							<HeaderCell>Last Login</HeaderCell>
							<Cell dataKey="lastLogin" />
						</Column>
					</Table>
					<div>
						<Pagination
							prev
							next
							first
							last
							ellipsis
							boundaryLinks
							maxButtons={5}
							size="xs"
							layout={["limit", "|", "pager", "skip"]}
							total={players.length}
							limitOptions={[5, 10, 15]}
							limit={limit}
							activePage={page}
							onChangePage={setPage}
							onChangeLimit={handleChangeLimit}
						/>
					</div>
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
