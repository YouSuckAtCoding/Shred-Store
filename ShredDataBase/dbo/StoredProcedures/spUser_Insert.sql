CREATE PROCEDURE [dbo].[spUser_Insert]
	
	@Name nvarchar(50),
	@Email nvarchar(200),
	@Password nvarchar(50),
	@Role nvarchar(50)
	
AS
BEGIN
	
	SET NOCOUNT ON
	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
	
	Insert into dbo.[User] (Name, Email, Password, Role, Salt) 
	Values(@Name, @Email, HASHBYTES('SHA2_512', @Password+CAST(@Salt as NVARCHAR(36))), @Role, @Salt)


END
