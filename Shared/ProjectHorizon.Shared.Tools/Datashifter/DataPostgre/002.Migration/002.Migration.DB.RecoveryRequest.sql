CREATE TABLE IF NOT EXISTS recoveryrequest (
    ID UUID DEFAULT uuid_generate_v4() PRIMARY KEY,
    userid UUID NOT NULL,
	recoverykey VARCHAR(8) NOT NULL,
    createddate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    expirydate TIMESTAMP DEFAULT CURRENT_TIMESTAMP + INTERVAL '5 minutes'
);