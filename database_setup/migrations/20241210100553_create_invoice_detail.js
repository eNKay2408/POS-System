/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.up = async function(knex) {
  await knex.raw(`
    CREATE TABLE IF NOT EXISTS invoice_detail (
    id SERIAL PRIMARY KEY,
    invoiceid INT NOT NULL,
    productid INT NOT NULL,
    quantity INT NOT NULL,
    FOREIGN KEY (invoiceid) REFERENCES invoice(id),
    FOREIGN KEY (productid) REFERENCES product(id)
    );
    `)
};

/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> }
 */
exports.down = async function(knex) {
  await knex.raw(`DROP TABLE IF EXISTS invoice_detail;`)
};
