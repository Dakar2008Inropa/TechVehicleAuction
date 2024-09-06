CREATE TABLE ActiveAuctions (
		Id Int PRIMARY KEY IDENTITY (1,1),
		SellerId Int NOT NULL,
		HighestBidderId Int,
		VehicleId NVARCHAR(100) NOT NULL,
		AskingPrice Decimal,
		CurrentBid Decimal,
		CreatedAt DateTime2 DEFAULT SYSDATETIME() );