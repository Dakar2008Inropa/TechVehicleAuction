
CREATE PROCEDURE CreateUser (@username TEXT, @sql TexT)

AS

BEGIN

SET @username = 'mitBrugernavn'

SET @sql = 'CREATE LOGIN ' + @username + ' WITH PASSWORD = ''' + @username + ''', DEFAULT_DATABASE='+@username+' , CHECK_POLICY = OFF;'

EXEC @sql;

END

GO;