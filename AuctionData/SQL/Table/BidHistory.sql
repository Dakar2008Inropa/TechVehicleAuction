﻿CREATE TABLE BidHistory (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Bid Decimal,
		UserId NVARCHAR(100),
		AuctionId Int,
		CreatedAt DATETIME DEFAULT SYSDATETIME(),
		FOREIGN KEY (UserId) REFERENCES Users (Id),
		FOREIGN KEY (AuctionId) REFERENCES Auctions (Id) );