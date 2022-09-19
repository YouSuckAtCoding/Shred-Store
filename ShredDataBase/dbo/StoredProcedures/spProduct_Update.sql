CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Name nvarchar(50),
	@Price money,
	@ImageName nvarchar(50)
AS
Begin
	
	Update dbo.[Product]
	Set Name = @Name, Price = @Price, ImageName = @ImageName
	Where Id = @Id;

End
