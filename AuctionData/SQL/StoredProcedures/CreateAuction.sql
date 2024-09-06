DROP PROCEDURE IF EXISTS CreateAuction

GO;

CREATE PROCEDURE CreateAuction
(
	@VehicleId		VARCHAR(100),
	@SellerId		VARCHAR(100),
	@AskingPrice	Decimal
)
AS
BEGIN;

SET NOCOUNT ON

INSERT INTO ActiveAuctions (VehicleId, SellerId, AskingPrice)
VALUES (@MovieId, @SellerId, @AskingPrice);

SET NOCOUNT OFF;

END;