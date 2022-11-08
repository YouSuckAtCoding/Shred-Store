CREATE PROCEDURE [dbo].[spCartItem_DeleteAll]
	@CartId int
AS
	
Begin
	
	Delete from dbo.[CartItem] 
	Where CartId = @CartId;

End
