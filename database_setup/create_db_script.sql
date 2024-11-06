-- Connect to the default database
\c postgres;

-- Create the new database
CREATE DATABASE POSSystem;

-- Connect to the new database
\c POSSystem;

-- Create the category table
CREATE TABLE category (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

-- Create the brand table
CREATE TABLE brand (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

-- Create the product table with foreign keys
CREATE TABLE product (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price numeric(18, 2) NOT NULL,
    categoryid INT NOT NULL,
    brandid INT,
    stock INT NOT NULL,
    FOREIGN KEY (categoryid) REFERENCES category(id),
    FOREIGN KEY (brandid) REFERENCES brand(id)
);

-- Create the employee table
CREATE TABLE employee (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100), 
    email VARCHAR(100),
    password VARCHAR(255)
);

-- To run this script, you can save it to a file (e.g., create_db.sql) and execute it using the psql command-line tool:
-- psql -U postgres -f create_db.sql
-- Replace your_username with your PostgreSQL username. You will be prompted to enter your password.