CREATE PROCEDURE [dbo].[spCart_GetById]
	@Id int
AS
Begin
	
	SET NOCOUNT ON
	Select Id, UserId from dbo.[Cart] 
	where Id = @Id;

End
