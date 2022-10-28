CREATE PROCEDURE [dbo].[spCart_GetById]
	@Id int
AS
Begin
	
	SET NOCOUNT ON
	Select Id, UserId, CreatedDate from dbo.[Cart] 
	where UserId = @Id;

End
