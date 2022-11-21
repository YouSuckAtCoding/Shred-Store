CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
Begin

	Select Id, Name, UserId, Brand, Price, [Description], category, ImageName from dbo.[Product]
	Where Id = @Id;

End
