import { useEffect, useState } from "react";
import { Territory } from "../../types/Territory";
import Loader from "../misc/Loader";
import { FlexboxGrid, Table } from "rsuite";
import { whatMapId } from "../../utils/helpers";

const { Column, HeaderCell, Cell } = Table;

export default function Territories({ mapId }: any) {
	const [territoryData, setTerritoryData] = useState<Territory[]>([]);
	const [sortColumn, setSortColumn] = useState("posX");
	const [sortType, setSortType] = useState();

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/territories/${mapId}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => setTerritoryData(res))
			.catch(err => console.error(err));
	}, []);

	const handleSortColumn = (sortColumn: any, sortType: any) => {
		setSortColumn(sortColumn);
		setSortType(sortType);
	};

	const getData = () => {
		if (sortColumn && sortType) {
			return territoryData.sort((a, b) => {
				let x: string | number = a[sortColumn as keyof typeof territoryData[0]];
				let y: string | number = b[sortColumn as keyof typeof territoryData[0]];

				if (typeof x === "string") x = x.charCodeAt(0);
				if (typeof y === "string") y = y.charCodeAt(0);

				if (sortType === "asc") return x - y;
				else return y - x;
			});
		}
		return territoryData;
	};

	if (territoryData.length === 0) return <Loader />;
	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={20}>
					<Table
						height={territoryData.length * 46 + 100}
						width={1275}
						data={getData()}
						hover={true}
						sortColumn={sortColumn}
						sortType={sortType}
						onSortColumn={handleSortColumn}
					>
						<Column width={225}>
							<HeaderCell>Last found</HeaderCell>
							<Cell dataKey="lastFound" />
						</Column>
						<Column width={150} sortable>
							<HeaderCell>Owner Name</HeaderCell>
							<Cell dataKey="ownerPlayerName" />
						</Column>
						<Column width={150} align="center" fixed sortable>
							<HeaderCell>Owner Player Id</HeaderCell>
							<Cell dataKey="ownerPlayerId" />
						</Column>
						<Column width={400} sortable>
							<HeaderCell>Owner Country</HeaderCell>
							<Cell dataKey="ownerCountry" />
						</Column>

						<Column width={100} sortable>
							<HeaderCell>Pos X</HeaderCell>
							<Cell dataKey="posX" />
						</Column>
						<Column width={150}>
							<HeaderCell>Pos Y</HeaderCell>
							<Cell dataKey="posZ" />
						</Column>
						<Column width={100} sortable>
							<HeaderCell>Pos Z</HeaderCell>
							<Cell dataKey="posY" />
						</Column>
					</Table>
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
