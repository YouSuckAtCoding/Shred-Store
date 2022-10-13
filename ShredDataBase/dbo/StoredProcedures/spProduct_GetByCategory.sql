﻿CREATE PROCEDURE [dbo].[spProduct_GetByCategory]
	@Category nvarchar(100)

AS
Begin

	Select Id, Name, UserId, Price, [Description], Category, ImageName from dbo.[Product]
	Where Category = @Category;

End
