export function compare(a: string, b: string) {
	const nameA = a.toUpperCase();
	const nameB = b.toUpperCase();

	if (nameA < nameB) return -1;
	if (nameA > nameB) return 1;

	return 0;
}

export function makeId(length: number): string {
	const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	const charactersLength = characters.length;

	let result = "";
	for (let i = 0; i < length; i++) result += characters.charAt(Math.floor(Math.random() * charactersLength));

	return result;
}

export function getAvatarUrl(hash: string, userId: string): string {
	return `https://cdn.discordapp.com/avatars/${userId}/${hash}.${hash.startsWith("a_") ? "gif" : "png"}?size=4096`;
}

export function whatMapId(name: string): string {
	if (name === "Deer Isle") return "0";
	if (name === "Chernarus") return "1";

	return "0";
}
