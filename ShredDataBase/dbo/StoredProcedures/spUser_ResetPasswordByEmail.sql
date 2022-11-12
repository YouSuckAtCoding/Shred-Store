CREATE PROCEDURE [dbo].[spUser_ResetPasswordByEmail]
	@Email nvarchar(200),
	@NewPassword nvarchar(50)
AS
Begin
	
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
	
	Update dbo.[User] Set Password = HASHBYTES('SHA2_512', @NewPassword+CAST(@Salt as NVARCHAR(36))),
	Salt = @Salt
	Where Email = @Email;

End