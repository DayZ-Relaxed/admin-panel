import { DiscordUser } from "../Discord";

export interface NavBarComponent {
	user: DiscordUser | undefined;
	updateMap: any;
}
