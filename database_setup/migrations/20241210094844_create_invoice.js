/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
  await knex.raw(`
    CREATE TABLE IF NOT EXISTS invoice (
    id SERIAL PRIMARY KEY,
    employeeid INT NOT NULL,
    total numeric(18, 2) NOT NULL,
    timestamp TIMESTAMP WITHOUT TIME ZONE DEFAULT NOW(),
    FOREIGN KEY (employeeid) REFERENCES employee(id)
);
    `)
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
  await knex.raw(`DROP TABLE IF EXISTS invoice;`)
};
