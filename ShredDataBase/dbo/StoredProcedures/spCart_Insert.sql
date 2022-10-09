CREATE PROCEDURE [dbo].[spCart_Insert]
	@UserId int,
	@CreatedDate datetime

AS
Begin
	
	Insert into dbo.[Cart] ( UserId, CreatedDate)
	Values ( @UserId, @CreatedDate)

End
