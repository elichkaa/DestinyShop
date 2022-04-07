import React from "react";
import api from "./api";

const Login = () => {
	let axiosConfig = {
		headers: {
			"Content-Type": "application/json;charset=UTF-8",
			"Access-Control-Allow-Origin": "*",
		},
	};

	const loginUser = async (username, password) => {
		await api
			.post("api/auth/login", { username, password }, axiosConfig)
			.then((res) => {
				if (res.status === 200) {
					window.location = "/";
				}
			})
			.catch((err) => {
				console.log("AXIOS ERROR: ", err);
			});
	};

	return (
		<div class="page-login">
			<div class="ui centered grid container">
				<div class="nine wide column">
					<div class="ui fluid card">
						<div class="content">
							<form class="ui form" method="POST">
								<div class="field">
									<label>Username</label>
									<input
										required
										minLength="6"
										type="text"
										name="username"
										placeholder="Username"
									/>
								</div>
								<div class="field">
									<label>Password</label>
									<input
										required
										type="password"
										name="password"
										placeholder="Password"
									/>
								</div>
								<button
									onClick={loginUser}
									class="ui primary icon button"
									type="submit"
								>
									Login
								</button>
							</form>
						</div>
					</div>
				</div>
			</div>
		</div>
	);
};

export default Login;
