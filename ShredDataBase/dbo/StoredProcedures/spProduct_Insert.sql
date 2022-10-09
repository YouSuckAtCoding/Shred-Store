CREATE PROCEDURE [dbo].[spProduct_Insert]
	@Name nvarchar(50),
	@UserId int,
	@Price money,
	@ImageName nvarchar(50)
AS
Begin

	Insert into dbo.[Product] (Name, UserId, Price, ImageName)
	values (@Name, @UserId, @Price, @ImageName);

End
