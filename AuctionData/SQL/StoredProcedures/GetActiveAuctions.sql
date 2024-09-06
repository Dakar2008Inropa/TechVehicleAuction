DROP PROCEDURE IF EXISTS GetActiveAuctions;

GO;

CREATE PROCEDURE GetActiveAuctions
(
	@Username	NVARCHAR(100)
)
AS
BEGIN;

SELECT
	AA.CurrentBid
	AA.AskingPrice