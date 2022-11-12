CREATE PROCEDURE [dbo].[spUser_CheckEmail]
	@Email nvarchar(200)
AS
Begin

	If(Exists(Select Email from dbo.[User] where Email = @Email))
	Select Email from dbo.[User] where Email = @Email
	
	
End
