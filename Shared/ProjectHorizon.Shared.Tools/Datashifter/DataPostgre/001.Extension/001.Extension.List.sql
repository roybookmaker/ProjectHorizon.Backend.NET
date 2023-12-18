-- Install pgcrypto extension for cryptographic functions
CREATE EXTENSION IF NOT EXISTS pgcrypto; -- Useful for encryption, hashing, and decryption
-- Sample usage:
-- SELECT crypt('sensitive_data', gen_salt('bf'));

-- Install hstore extension for key-value pair storage
CREATE EXTENSION IF NOT EXISTS hstore; -- Ideal for schema-less storage, dynamic properties
-- Sample usage:
-- CREATE TABLE products (id SERIAL PRIMARY KEY, name VARCHAR(100), properties HSTORE);
-- INSERT INTO products (name, properties) VALUES ('Product 1', 'color => red, size => large');
-- SELECT * FROM products WHERE properties @> '"size"=>"large"';

-- Install pg_trgm extension for fuzzy text searching
CREATE EXTENSION IF NOT EXISTS pg_trgm; -- Handy for similarity checking and search functionality
-- Sample usage:
-- CREATE INDEX idx_similar_names ON employees USING gin (name gin_trgm_ops);
-- SELECT name FROM employees WHERE name % 'John';

-- Install unaccent extension for accent-insensitive searches
CREATE EXTENSION IF NOT EXISTS unaccent; -- Useful for ignoring accents in text search
-- Sample usage:
-- SELECT unaccent('Café') AS normalized_text;
-- SELECT * FROM products WHERE unaccent(name) ILIKE unaccent('%cafe%');

-- Install citext extension for case-insensitive text comparison
CREATE EXTENSION IF NOT EXISTS citext; -- Facilitates case-insensitive comparisons
-- Sample usage:
-- CREATE TABLE users (id SERIAL PRIMARY KEY, username CITEXT);
-- INSERT INTO users (username) VALUES ('JohnDoe'), ('janedoe');
-- SELECT * FROM users WHERE username = 'johndoe';

-- Install uuid-ossp extension for UUID generation
CREATE EXTENSION IF NOT EXISTS "uuid-ossp"; -- Generates universally unique identifiers
-- Sample usage:
-- CREATE TABLE accounts (id UUID DEFAULT uuid_generate_v4() PRIMARY KEY, ...);