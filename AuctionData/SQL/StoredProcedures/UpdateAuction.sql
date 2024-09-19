DROP PROCEDURE IF EXISTS UpdateAuction

GO;

CREATE PROCEDURE UpdateAuction
(
	@AuctionId		INT,
	@AskingPrice	Decimal
)
AS
BEGIN;

SET NOCOUNT ON
IF EXISTS (SELECT 1 FROM Auctions WHERE Id = @AuctionId AND HighestBidderId != NULL)
THROW 51000, 'Cant edit an auction with an active bid', 1;

UPDATE Auctions SET AskingPrice = @AskingPrice WHERE Id = @AuctionId

SET NOCOUNT OFF;

END;