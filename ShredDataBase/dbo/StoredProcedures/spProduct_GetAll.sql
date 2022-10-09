CREATE PROCEDURE [dbo].[spProduct_GetAll]
	
AS
Begin

	Select Id, Name, UserId, Price, ImageName from dbo.[Product] 


End
