﻿DROP PROCEDURE IF EXISTS GetUser;	

GO

CREATE PROCEDURE GetUser
(
	@Username	NVARCHAR (50)
)
AS
BEGIN;

SET NOCOUNT ON

SELECT * FROM dbo.Users WHERE Username = @Username;

SET NOCOUNT OFF;

END;