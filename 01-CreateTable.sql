
CREATE DATABASE carrefourDb
GO

USE carrefourDb
GO

-- CreateTable
CREATE TABLE dbo.transactions (
    id VARCHAR(50) NOT NULL,
    amount VARCHAR(20) NOT NULL,
    description TEXT DEFAULT NULL,
    insertion_date CHAR(10) NOT NULL,
    CONSTRAINT transaction_pkey PRIMARY KEY (id)
)
GO