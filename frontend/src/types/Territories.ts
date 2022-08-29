import { PlayerPositionInTerritory, Player } from "./Player";

export interface TerritoriesVisitsTableComponent {
	territoryPassed: PlayerPositionInTerritory[];
	showMembers: boolean;
	territoryMembers: Player[];
}

export interface TerritoriesMembersTableComponent {
	territoryMembers: Player[];
}
