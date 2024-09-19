
CREATE PROCEDURE CreateUser (@username TEXT, @sql TexT)

AS

BEGIN

SET @sql = 'CREATE LOGIN ' + @username + ' WITH PASSWORD = ''' + @username + ''', DEFAULT_DATABASE='+@username+' , CHECK_POLICY = OFF;'

EXEC @sql;

END

GO;