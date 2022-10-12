CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
AS
Begin

	Select Id, Name, UserId, Price, [Description], Category, ImageName from dbo.[Product] 


End
