import React from "react";
import "./css/style.css";
import Link from "./Link";

const Navbar = () => {
	return (
		<div style={{ padding: 20 }} id="top-header">
			<div class="ui container">
				<div class="ui grid">
					<ul class="header-links left floated six wide column">
						<li>
							<Link href="/" className="item">
								<i class="fa-solid fa-house"></i> Home
							</Link>
						</li>
						<li>
							<Link href="privacy" className="item">
								<i class="fa-solid fa-lock"></i> Privacy
							</Link>
						</li>
						<li>
							<Link href="store" className="item">
								<i class="fa-solid fa-shop"></i>Store
							</Link>
						</li>
						<li>
							<Link href="menu" className="item">
								<i class="fa-solid fa-bars"></i>Menu
							</Link>
						</li>
					</ul>
					<ul class="header-links right floated three wide column">
						<li>
							<Link href="/api/auth/login" className="item">
								<i class="fa-solid fa-key"></i>Login
							</Link>
						</li>
						<li>
							<Link href="/api/auth/register" className="item">
								<i class="fa-solid fa-door-closed"></i>
								Register
							</Link>
						</li>
					</ul>
				</div>
			</div>
		</div>
	);
};

export default Navbar;
