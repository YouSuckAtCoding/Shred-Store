CREATE PROCEDURE [dbo].[spCartItem_GetById]
	@Id int
	
AS
Begin

	Select Id, CartId, ProductId from dbo.[CartItem] 
	where Id = @Id;


End
