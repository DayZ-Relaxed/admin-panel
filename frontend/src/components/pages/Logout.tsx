import { Navigate } from "react-router-dom";
import { useCookies } from "react-cookie";
import { useEffect } from "react";

export default function LogoutPage() {
	const [_cookie, _setCookie, removeCookie] = useCookies(["token"]);

	useEffect(() => {
		removeCookie("token");
	}, []);

	return <Navigate to={"/"} />;
}
