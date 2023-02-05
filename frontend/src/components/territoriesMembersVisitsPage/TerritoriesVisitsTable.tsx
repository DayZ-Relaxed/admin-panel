import { FlexboxGrid, Table, Button } from "rsuite";
import { TerritoriesVisitsTableComponent } from "../../types/Territories";

const { Column, HeaderCell, Cell } = Table;

export function TerritoriesVisitsTable({ territoryPassed, showMembers, territoryMembers, loading }: TerritoriesVisitsTableComponent) {
	const data = territoryPassed.filter(log => {
		if (showMembers) return log;
		if (!territoryMembers.find(player => player.playerName === log.playerName)) return log;
		return null;
	});

	return (
		<>
			<FlexboxGrid justify="center">
				<FlexboxGrid.Item colspan={20}>
					<h4>Territory visits ({data.length})</h4>
				</FlexboxGrid.Item>
			</FlexboxGrid>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={20}>
					<Table height={data.length * 46 + 100} width={1250} data={data} hover={true} loading={loading}>
						<Column width={150} align="center" fixed>
							<HeaderCell>Date</HeaderCell>
							<Cell dataKey="date" />
						</Column>

						<Column width={150}>
							<HeaderCell>Time</HeaderCell>
							<Cell dataKey="time" />
						</Column>

						<Column width={350}>
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
						<Column width={100}>
							<HeaderCell>Pos Z</HeaderCell>
							<Cell dataKey="posY" />
						</Column>

						<Column width={300}>
							<HeaderCell>...</HeaderCell>

							<Cell style={{ padding: "11px" }}>{row => <code>{`/tpp ${row.posX}, ${row.posZ}, ${row.posY}`}</code>}</Cell>
						</Column>
					</Table>
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
