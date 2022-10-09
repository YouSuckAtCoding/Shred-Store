CREATE PROCEDURE [dbo].[spCartItem_Insert]
	@CartId int, 
	@ProductId int
AS
Begin

	Insert into dbo.[CartItem] (CartId, ProductId)
	Values (@CartId, @ProductId)

End
