DROP PROCEDURE IF EXISTS CreateBid

GO;

CREATE PROCEDURE CreateBid
(
	@AuctionId		INT,
	@BidderId		INT,
	@Bid			Decimal
)
AS
BEGIN;

SET NOCOUNT ON

DECLARE @BaseId INT;
EXEC @BaseId = GetBaseReference

INSERT INTO BidHistory (Bid, UserId, AuctionId, BaseId) VALUES (@Bid, @UserId, @BidderId, @BaseId)

SET NOCOUNT OFF;

END;