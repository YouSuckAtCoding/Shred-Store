CREATE PROCEDURE [dbo].[spCartItem_GetAll]
	@CartId int
AS
Begin

	Select Id, CartId, ProductId from dbo.[CartItem]
	Where CartId = @CartId;


End