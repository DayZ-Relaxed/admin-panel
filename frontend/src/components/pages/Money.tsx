import { useEffect, useState } from "react";
import { Territory } from "../../types/Territory";
import Loader from "../misc/Loader";
import { Button, FlexboxGrid, Input, Table } from "rsuite";
import { whatMapId } from "../../utils/helpers";
import { VehicleDamage } from "../../types/VehicleDamage";

const { Column, HeaderCell, Cell } = Table;

export default function MoneyPage({ mapId }: any) {
	const [isLoading, setIsLoading] = useState(true);
	const [data, setData] = useState<any[]>([]);
	const [sortColumn, setSortColumn] = useState("posX");
	const [sortType, setSortType] = useState();

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/money/${mapId}`, {
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
	}, []);

	const handleSortColumn = (sortColumn: any, sortType: any) => {
		setSortColumn(sortColumn);
		setSortType(sortType);
	};

	const getData = () => {
		if (sortColumn && sortType) {
			return data.sort((a, b) => {
				let x: string | number = a[sortColumn as keyof typeof data[0]];
				let y: string | number = b[sortColumn as keyof typeof data[0]];

				if (typeof x === "string") x = x.charCodeAt(0);
				if (typeof y === "string") y = y.charCodeAt(0);

				if (sortType === "asc") return x - y;
				else return y - x;
			});
		}
		return data;
	};

	return (
		<>
			<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
				<FlexboxGrid.Item colspan={20}>
					<Table
						height={data.length * 46 + 100}
						width={550}
						data={getData()}
						hover={true}
						loading={isLoading}
						sortColumn={sortColumn}
						sortType={sortType}
						onSortColumn={handleSortColumn}
					>
						<Column width={200} sortable>
							<HeaderCell>Player Name</HeaderCell>
							<Cell dataKey="name" />
						</Column>

						<Column width={200}>
							<HeaderCell>Steam Id</HeaderCell>
							<Cell dataKey="steamId" />
						</Column>
						<Column width={150} sortable>
							<HeaderCell>Money</HeaderCell>
							<Cell dataKey="moneyAmount" />
						</Column>
					</Table>
				</FlexboxGrid.Item>
			</FlexboxGrid>
		</>
	);
}
