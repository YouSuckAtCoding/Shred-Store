CREATE PROCEDURE [dbo].[spUser_GetAll]


AS
Begin
	
	SET NOCOUNT ON
	SELECT * FROM dbo.[User]

End
	

