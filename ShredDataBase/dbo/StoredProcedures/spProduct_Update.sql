CREATE PROCEDURE [dbo].[spProduct_Update]
	@Id int,
	@Name nvarchar(50),
	@Brand nvarchar(50),
	@UserId int,
	@Price money,
	@Description nvarchar(300),
	@Category nvarchar(50),
	@ImageName nvarchar(50)
AS
Begin

	Update dbo.[Product]
	Set [Name] = @Name,
	UserId = @UserId,
	Brand = @Brand,
	Price = @Price,
	[Description] = @Description,
	Category = @Category,
	ImageName = @ImageName
	Where Id = @Id;

End
