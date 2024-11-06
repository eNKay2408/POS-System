// Update with your config settings.
require("dotenv").config();
/**
 * @type { Object.<string, import("knex").Knex.Config> }
 */
module.exports = {
	development: {
		client: "postgresql",
		connection: {
			host: `${process.env.DB_HOST}`,
			user: `${process.env.DB_USERNAME}`,
			password: `${process.env.DB_PASSWORD}`,
			database: `${process.env.DB_NAME}`,
			port: `${process.env.DB_PORT}`,
			charset: "utf8",
		},
	},
	migrations: {
		directory: "/knex/migrations",
	},
};
