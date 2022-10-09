CREATE PROCEDURE [dbo].[spProduct_Delete]
	@Id int
AS
Begin

		Delete from dbo.[Product]
		Where Id = @Id;

End
