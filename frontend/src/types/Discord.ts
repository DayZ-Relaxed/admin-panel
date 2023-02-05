export interface DiscordUser {
	id: string;
	username: string;
	discriminator: string;
	avatar: string;
	mfa_enabled: boolean;
	banner: string;
	accent_color: string;
	locale: string;
	flags: number;
	premium_type: number;
	public_flags: number;
}
