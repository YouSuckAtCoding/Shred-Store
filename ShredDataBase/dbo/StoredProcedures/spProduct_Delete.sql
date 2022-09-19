CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int
AS
Begin

	Declare @AdmCheck int
	Exec dbo.spCheckUserRole @Id, @IsAdm = @AdmCheck Output
	If @AdmCheck = 1
	Begin
		Delete from dbo.[Product]
		Where Id = @Id;
	End
		
	
End
