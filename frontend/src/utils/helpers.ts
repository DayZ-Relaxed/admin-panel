export function compare(a: string, b: string) {
	const nameA = a.toUpperCase();
	const nameB = b.toUpperCase();

	if (nameA < nameB) return -1;
	if (nameA > nameB) return 1;

	return 0;
}

export function makeId(length: number) {
	const characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
	const charactersLength = characters.length;

	let result = "";
	for (let i = 0; i < length; i++) result += characters.charAt(Math.floor(Math.random() * charactersLength));

	return result;
}
