CREATE PROCEDURE [dbo].[spUser_Login]
	@Name nvarchar(50),
	@Password nvarchar(50),
	@ResponseMessage nvarchar(250)='' Output

AS
Begin
	
	SET NOCOUNT ON

	Declare @SelectedId int

	IF Exists (Select Top 1 Id From dbo.[User] Where Name = @Name)
	Begin
		Set @SelectedId=(Select Id from dbo.[User] Where Name = @Name And Password=HASHBYTES('SHA2_512',@Password+Cast(Salt As nvarchar(36))))

		If(@SelectedId Is Null)
			Set @ResponseMessage='Incorret Password'
		Else
			Set @ResponseMessage='Login Successfull'

	End
	Else
		Set @ResponseMessage='Invalid Login Attempt'

End
	


