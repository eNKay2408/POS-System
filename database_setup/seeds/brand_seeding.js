/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('brand').del()
  await knex('brand').insert([
    {id:1, name: 'LV'},
    {id:2, name: 'Gucci'},
    {id:3, name: 'Chanel'},
  ]);
};
