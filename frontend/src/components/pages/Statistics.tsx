import { useEffect, useState } from "react";
import Loader from "../misc/Loader";
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend } from "chart.js";
import { Line } from "react-chartjs-2";
import { Statistics } from "../../types/Statistics";
ChartJS.register(CategoryScale, LinearScale, PointElement, LineElement, Title, Tooltip, Legend);

export default function StatisticsPage({ mapId }: any) {
	const [statistics, setStatistics] = useState<Statistics[]>([]);
	const [isLoading, setIsLoading] = useState(true);

	useEffect(() => {
		fetch(`${process.env.REACT_APP_API_URL}/statistics/${mapId}`, {
			credentials: "include",
		})
			.then(res => res.json())
			.then(res => {
				setStatistics(res);
				setIsLoading(false);
			})
			.catch(err => {
				console.error(err);
				setIsLoading(false);
			});
	}, []);

	const options = {
		responsive: true,
		plugins: {
			legend: {
				position: "top" as const,
			},
			title: {
				display: true,
				text: "Map Statistics",
			},
		},
	};

	let data = {
		labels: statistics.filter(s => s.description === "Number of territories").map(s => s.dateWritten),
		datasets: [
			{
				label: "Number of territories",
				data: statistics.filter(s => s.description === "Number of territories").map(s => s.value),
				borderColor: "rgb(53, 162, 235)",
				backgroundColor: "rgba(53, 162, 235, 0.5)",
			},
		],
	};

	if (isLoading) return <Loader />;
	return (
		<>
			<Line options={options} data={data} />
		</>
	);
}
