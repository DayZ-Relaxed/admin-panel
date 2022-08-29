export function generateOauthLink(): string {
	const SCOPES = ["identify"].join("%20");
	//const STATE = makeId(10); &state=${STATE}

	return `https://discord.com/api/oauth2/authorize?response_type=code&client_id=${process.env.REACT_APP_CLIENT_ID}&scope=${SCOPES}&redirect_uri=${process.env.REACT_APP_REDIRECT_URI}`;
}
