CREATE TABLE BidHistory (
		Id INT PRIMARY KEY IDENTITY(1,1),
		Bid Decimal,
		UserId NVARCHAR(100),
		AuctionId Int,
		FOREIGN KEY (UserId) REFERENCES Users (Id),
		FOREIGN KEY AuctionId Int
		--Jeg kan ikke sætte en foreign key på ActiveAuctions da man på et tidspunkt gerne vil slette den record og sætte den over i completed auctions