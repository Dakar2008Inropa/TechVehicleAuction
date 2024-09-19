DROP PROCEDURE IF EXISTS SubmitBid

GO;

CREATE PROCEDURE SubmitBid
(
	@AuctionId		INT,
	@BidderId		INT,
	@Bid			Decimal
)
AS
BEGIN;

SET NOCOUNT ON

UPDATE Auctions SET HighestBidderId = @BidderId, CurrentBid = @Bid
WHERE Id = @AuctionId

EXEC CreateBid @AuctionId, @BidderId, @Bid

SET NOCOUNT OFF;

END;