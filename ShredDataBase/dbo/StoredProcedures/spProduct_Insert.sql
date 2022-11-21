CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name nvarchar(50),
	@Brand nvarchar(50),
	@UserId int,
	@Price money,
	@Description nvarchar(300),
	@Category nvarchar(50),
	@ImageName nvarchar(50)
AS
Begin

	Insert into dbo.[Product] (Name, Brand, UserId, Price, [Description], Category, ImageName)
	values (@Name, @Brand, @UserId, @Price, @Description, @Category, @ImageName);

End
