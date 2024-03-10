-- Create Database
CREATE DATABASE TopUpDatabase;
GO

-- Use the created database
USE TopUpDatabase;
GO

-- Create Beneficiaries Table
CREATE TABLE Beneficiaries (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PhoneNumber NVARCHAR(20) NOT NULL,
    Nickname NVARCHAR(20) NOT NULL,
	UserId INT NOT NULL
);

--Create Topup options table

CREATE TABLE TopUpOptions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(18, 2) NOT NULL,
    [status] NVARCHAR(2) NOT NULL 
);


-- Create TopUpTransactions Table
CREATE TABLE TopUpTransactions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    BeneficiaryId INT NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    TransactionDate DATETIME NOT NULL,
	UserId INT NOT NULL
);


-- Create Users Table
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
	UserName  NVARCHAR(20) NOT NULL,
   Balance DECIMAL(18, 2) NOT NULL
);



INSERT INTO Users (Username,Balance)
VALUES
('user1', 1000),
('user2', 1500),
('user3', 800),
('user4', 2000),
('user5', 1200),
('user6', 500),
('user7', 3000),
('user8', 700),
('user9',  1800),
('user10',  2500);

-- Insert Sample TopUpTransactions



INSERT INTO TopUpOptions (Amount, [status]) VALUES
(5.00, 'Y'),
(10.00, 'Y'),
(20.00,'Y'),
(30.00, 'Y'),
(50.00, 'Y'),
(75.00, 'Y'),
(100.00, 'Y');






