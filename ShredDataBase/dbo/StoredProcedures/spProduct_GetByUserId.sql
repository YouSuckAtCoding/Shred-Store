CREATE PROCEDURE [dbo].[spProduct_GetByUserId]
	@UserId int
AS
Begin
	
	Select Id, Name, UserId, Price, [Description], category, ImageName from dbo.[Product]
	Where UserId = @UserId;
	
End
