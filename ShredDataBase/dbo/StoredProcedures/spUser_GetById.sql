CREATE PROCEDURE [dbo].[spUser_GetById]
	@Id int
AS
BEGIN
	
	SET NOCOUNT ON
	Select * from dbo.[User] 
	where Id = @Id;


END

