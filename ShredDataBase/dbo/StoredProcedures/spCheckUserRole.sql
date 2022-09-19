CREATE PROCEDURE [dbo].[spCheckUserRole]
	@Id int,
	@IsAdm int Output
AS
Begin
	
	Set NOCOUNT ON
	
	If Exists (Select Top 1 Id From dbo.[User] Where Id = @Id)
	Begin
		If Exists (Select Role From dbo.[User] Where Id = @Id and Role = 'Administrator')
			Set @IsAdm = 1
		Else
			Set @IsAdm = 0
	End
	Else
		Set @IsAdm = 0

	
	
	
End
