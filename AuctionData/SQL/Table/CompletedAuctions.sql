CREATE TABLE CompletedTransactions (
		Id INT PRIMARY KEY IDENTITY(1,1),
		BuyerId INT NOT NULL,
		SellerId INT NOT NULL,
		Price DECIMAL,
		BaseId INT,
		FOREIGN KEY (BaseId) REFERENCES Base(Id));