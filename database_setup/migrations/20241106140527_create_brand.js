/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
  await knex.raw(
    `CREATE TABLE IF NOT EXISTS brand 
    (
    ID SERIAL PRIMARY KEY,
    NAME VARCHAR(100) NOT NULL
  )`
  );
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
  await knex.raw(`DROP TABLE IF EXISTS brand`);
};
