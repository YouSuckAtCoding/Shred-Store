CREATE PROCEDURE [dbo].[spUser_GetAll]


AS
Begin
	
	SET NOCOUNT ON
	SELECT Id, Name, Email, Role FROM dbo.[User]

End
	

