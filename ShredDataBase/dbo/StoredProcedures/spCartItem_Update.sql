CREATE PROCEDURE [dbo].[spCartItem_Update]
	@Id int,
	@ProductId int
	
AS
Begin

	Update dbo.CartItem
	Set ProductId = @ProductId
	Where Id = @Id;

End
