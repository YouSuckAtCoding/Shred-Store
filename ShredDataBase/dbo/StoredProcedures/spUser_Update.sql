CREATE PROCEDURE [dbo].[spUser_Update]
	@Id int,
	@Name nvarchar(50),
	@Email nvarchar(200)
AS
Begin

	Update dbo.[User]
	Set Name = @Name, Email = @Email
	Where Id = @Id;

End
	

