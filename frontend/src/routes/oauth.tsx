import { useSearchParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import Loader from "../components/misc/Loader";

export default function Oauth() {
	const [searchParams, _] = useSearchParams();
	const [resp, setResp] = useState<string>("loading");

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/oauth/${searchParams.get("code")}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				if (res.token) setResp("success");
				else if (res.status !== 200) setResp("error");
				else setResp("error");
			})
			.catch(_ => setResp(`error`));
	}, []);

	if (resp === "loading") return <Loader />;
	else if (resp === "error") return <Navigate to={"/"} />;
	else return <Navigate to={"/"} />;
}
