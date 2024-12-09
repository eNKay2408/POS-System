/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
  await knex.raw(` 
    CREATE TABLE IF NOT EXISTS product (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price NUMERIC(18, 2) NOT NULL,
    stock INTEGER NOT NULL,
    categoryid INTEGER NOT NULL,
    brandid INTEGER,
    FOREIGN KEY (categoryid) REFERENCES category(id),
    FOREIGN KEY (brandid) REFERENCES brand(id))`);
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
  await knex.raw('DROP TABLE IF EXISTS product');
};
