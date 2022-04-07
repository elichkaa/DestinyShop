import React, { useState } from "react";
import api from "./api";
import Navbar from "./Navbar";
import Login from "./Login";
import Search from "./Search";
import Route from "./Route";

const App = () => {
	const [forecast, setForecast] = useState([]);

	const getForecast = async () => {
		const response = await api.get("/weatherforecast");
		setForecast(response.data);
	};

	const renderWeatherAsTable = () => {
		return (
			<table class="ui celled table">
				<thead>
					<tr>
						<th>Date</th>
						<th>TemperatureC</th>
						<th>Summary</th>
					</tr>
				</thead>
				<tbody>
					{forecast.map((single) => {
						return (
							<tr>
								<td data-label="Date">{single.date}</td>
								<td data-label="TemperatureC">
									{single.temperatureC}
								</td>
								<td data-label="Summary">{single.summary}</td>
							</tr>
						);
					})}
				</tbody>
			</table>
		);
	};

	return (
		<div>
			<Navbar />
			<Route path="/api/auth/login">
				<Login />
			</Route>
			{/* <h1>Weather Report</h1>
			<button onClick={getForecast}>Click me!</button>
			{forecast?.length > 0 && renderWeatherAsTable()} */}
		</div>
	);
};

export default App;
