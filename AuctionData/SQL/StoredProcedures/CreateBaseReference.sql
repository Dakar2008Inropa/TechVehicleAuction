﻿DROP PROCEDURE IF EXISTS CreateBaseReference
GO;

CREATE PROCEDURE CreateBaseReference
AS
BEGIN;

SET NOCOUNT ON

INSERT INTO Base DEFAULT VALUES

SET NOCOUNT OFF;

SELECT SCOPE_IDENTITY();
RETURN;

END;