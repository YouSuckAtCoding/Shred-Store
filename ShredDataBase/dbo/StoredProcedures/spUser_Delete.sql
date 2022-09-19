CREATE PROCEDURE [dbo].[spUser_Delete]
	@Id int
AS
Begin

	Delete from dbo.[User] 
	where Id = @Id;

End
