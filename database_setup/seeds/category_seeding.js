/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('category').del()
  await knex('category').insert([
    {id:1, name: 'Bag'},
    {id:2, name: 'Shoes'},
    {id:3, name: 'Clothes'},
  ]);
};
