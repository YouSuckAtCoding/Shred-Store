﻿CREATE TABLE [dbo].[Cart]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] INT NOT NULL FOREIGN KEY REFERENCES [User](Id),
    [CreatedDate] DATETIME NOT NULL
)
