CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id int
AS
Begin

	Select Id, Name, UserId, Price, ImageName from dbo.[Product]
	Where Id = @Id;

End
