export interface Player {
	playerId: number;
	playerName: string;
	battleEyeId: string;
	steamId: string;
	country: string;
	lastLogin: string;
}

export interface PlayerPositionInTerritory {
	date: string;
	time: string;
	playerName: string;
	dayzId: string;
	posX: number;
	posY: number;
	posZ: number;
}
