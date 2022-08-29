import { FlexboxGrid, Table } from "rsuite";
import { TerritoriesMembersTableComponent } from "../../types/Territories";

const { Column, HeaderCell, Cell } = Table;

export function TerritoriesMembersTable({ territoryMembers }: TerritoriesMembersTableComponent) {
	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 25 }}>
				<FlexboxGrid.Item colspan={6}>
					<h4>Territory members ({territoryMembers.length})</h4>
				</FlexboxGrid.Item>
			</FlexboxGrid>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={17}>
					<Table height={territoryMembers.length * 46 + 100} width={1000} data={territoryMembers} hover={true}>
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
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
