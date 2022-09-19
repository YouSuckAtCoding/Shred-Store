CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name nvarchar(50),
	@Price money,
	@ImageName nvarchar(50)
AS
Begin

	Insert into dbo.[Product] (Name, Price, ImageName)
	values (@Name, @Price, @ImageName);

End
