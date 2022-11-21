CREATE PROCEDURE [dbo].[spProduct_GetByUserId]
	@UserId int
AS
Begin
	
	Select Id, Name, UserId, Brand, Price, [Description], category, ImageName from dbo.[Product]
	Where UserId = @UserId;
	
End
