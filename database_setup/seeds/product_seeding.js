/**
 * @param { import("knex").Knex } knex
 * @returns { Promise<void> } 
 */
exports.seed = async function(knex) {
  // Deletes ALL existing entries
  await knex('product').del()
  await knex('product').insert([
    {id:1, name: 'LV Bag', price: 1000, categoryid: 1, brandid: 1, stock: 1000},
    {id:2, name: 'Gucci Shoes', price: 2000, categoryid: 2, brandid: 2, stock: 200},
    {id:3, name: 'Chanel Clothes', price: 3000, categoryid: 3, brandid: 3, stock: 300},
  ]);
};
