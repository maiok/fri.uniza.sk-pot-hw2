﻿CREATE TABLE [dbo].[Clubs]
(
	[ClubID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [Url] NVARCHAR(50) NOT NULL, 
)