const { createProxyMiddleware } = require("http-proxy-middleware");

const context = ["/api/auth/login"];

module.exports = function (app) {
	const appProxy = createProxyMiddleware(context, {
		target: "https://localhost:7230/",
		secure: false,
	});

	app.use(appProxy);
};
