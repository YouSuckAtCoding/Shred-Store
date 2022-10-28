CREATE PROCEDURE [dbo].[spCartItem_Delete]
	@ProductId int,
	@Amount int,
	@CartId int
AS
	
Begin
	
	Delete Top (@Amount) from dbo.[CartItem] 
	Where ProductId = @ProductId and CartId = @CartId;

End
