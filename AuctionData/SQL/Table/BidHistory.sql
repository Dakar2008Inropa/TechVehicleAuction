CREATE TABLE BidHistory (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Bid Decimal,
		UserId NVARCHAR(100),
		AuctionId Int,
		BaseId INT,
		FOREIGN KEY (BaseId) REFERENCES Base(Id),
		FOREIGN KEY (UserId) REFERENCES Users (Id),
		FOREIGN KEY (AuctionId) REFERENCES Auctions (Id) );