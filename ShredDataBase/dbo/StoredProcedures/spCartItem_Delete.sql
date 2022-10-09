CREATE PROCEDURE [dbo].[spCartItem_Delete]
	@Id int
AS
	
Begin
	
	Delete from dbo.[CartItem] 
	Where Id = @Id;

End
