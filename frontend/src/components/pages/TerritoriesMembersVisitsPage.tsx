import { SelectPicker, FlexboxGrid, Toggle } from "rsuite";
import { useEffect, useState } from "react";
import { Territory } from "../../types/Territory";
import { compare } from "../../utils/helpers";
import { Player, PlayerPositionInTerritory } from "../../types/Player";
import { TerritoriesMembersTable } from "../territoriesMembersVisitsPage/TerritoriesMembersTable";
import { TerritoriesVisitsTable } from "../territoriesMembersVisitsPage/TerritoriesVisitsTable";
import Loader from "../misc/Loader";

export default function TerritoriesMembersVisitsPage({ mapId }: any) {
	const [territoryData, setTerritoryData] = useState<Territory[]>([]);
	const [selectPickerData, setSelectPickerData] = useState([]);
	const [selectedTerritory, setSelectedTerritory] = useState<Territory | undefined>(undefined);
	const [selectedTerritoryMembers, setSelectedTerritoryMembers] = useState<Player[]>([]);
	const [selectedTerritoryPassed, setSelectedTerritoryPassed] = useState<PlayerPositionInTerritory[]>([]);
	const [showTerritoryMembers, setShowTerritoryMembers] = useState(false);
	const [loadingVisits, setLoadingVisits] = useState(false);

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/territories/${mapId}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				setSelectPickerData(
					res.map((territory: Territory) => ({
						label: territory.ownerPlayerName,
						value: territory.territoryId,
					})),
				);
				setTerritoryData(res);
			})
			.catch(err => console.error(err));
	}, []);

	if (territoryData.length === 0) return <Loader />;
	return (
		<>
			<FlexboxGrid justify="center">
				<FlexboxGrid.Item colspan={5}>
					<h4>Territory</h4>
					<SelectPicker
						disabled={selectPickerData.length === 0 ? true : false}
						data={selectPickerData}
						style={{ width: 224 }}
						searchable={true}
						sort={_ => {
							return (a, b) => compare(a.label, b.label);
						}}
						onSelect={(territoryId, _) => {
							setLoadingVisits(true);
							setSelectedTerritoryPassed([]);

							fetch(`${process.env.REACT_APP_API_URL}/getterritorymembers/${mapId}/${territoryId}`, {
								credentials: "include",
							})
								.then(res => res.json())
								.then(res => setSelectedTerritoryMembers(res));

							fetch(`${process.env.REACT_APP_API_URL}/getplayerposition/${mapId}/${territoryId}`, {
								credentials: "include",
							})
								.then(res => res.json())
								.then(res => {
									setSelectedTerritoryPassed(res);
									setLoadingVisits(false);
								});

							setSelectedTerritory(territoryData.find((territory: Territory) => territory.territoryId === territoryId));
						}}
						onClean={_event => {
							setSelectedTerritoryMembers([]);
							setSelectedTerritoryPassed([]);
							setSelectedTerritory(undefined);
						}}
					/>
				</FlexboxGrid.Item>

				<FlexboxGrid.Item colspan={15}>
					<h4>Show Territory Members</h4>
					<Toggle size="md" checkedChildren="Hide" unCheckedChildren="Show" onChange={checked => setShowTerritoryMembers(checked)} />
				</FlexboxGrid.Item>
			</FlexboxGrid>

			{selectedTerritory !== undefined ? (
				<>
					<FlexboxGrid justify="center" style={{ marginTop: 12 }}>
						<FlexboxGrid.Item colspan={20}>Last found: {selectedTerritory.lastFound}</FlexboxGrid.Item>
						<FlexboxGrid.Item colspan={20}>
							Pos {selectedTerritory.posX}, {selectedTerritory.posZ}, {selectedTerritory.posY}
						</FlexboxGrid.Item>
					</FlexboxGrid>

					<TerritoriesMembersTable territoryMembers={selectedTerritoryMembers} />
					<TerritoriesVisitsTable
						territoryPassed={selectedTerritoryPassed}
						showMembers={showTerritoryMembers}
						territoryMembers={selectedTerritoryMembers}
						loading={loadingVisits}
					/>
				</>
			) : (
				<> </>
			)}
		</>
	);
}
