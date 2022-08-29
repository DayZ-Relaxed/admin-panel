import { FlexboxGrid, Table } from "rsuite";
import { TerritoriesVisitsTableComponent } from "../../types/Territories";

const { Column, HeaderCell, Cell } = Table;

export function TerritoriesVisitsTable({ territoryPassed, showMembers, territoryMembers }: TerritoriesVisitsTableComponent) {
	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 25 }}>
				<FlexboxGrid.Item colspan={5}>
					<h4>Territory visits</h4>
				</FlexboxGrid.Item>
			</FlexboxGrid>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={17}>
					<Table
						height={400}
						width={1000}
						data={territoryPassed.filter(log => {
							if (showMembers) return log;
							if (!territoryMembers.find(player => player.playerName === log.playerName)) return log;
						})}
						hover={true}
					>
						<Column width={150} align="center" fixed>
							<HeaderCell>Date</HeaderCell>
							<Cell dataKey="date" />
						</Column>

						<Column width={150}>
							<HeaderCell>Time</HeaderCell>
							<Cell dataKey="time" />
						</Column>

						<Column width={400}>
							<HeaderCell>Player Name</HeaderCell>
							<Cell dataKey="playerName" />
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
