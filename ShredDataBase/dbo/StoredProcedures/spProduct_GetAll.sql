CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
AS
Begin

	Select Id, Name, UserId, Brand, Price, [Description], Category, ImageName from dbo.[Product] 


End
