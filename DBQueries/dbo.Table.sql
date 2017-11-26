CREATE TABLE [dbo].[Players]
(
	[PlayerID] INT NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(20) NOT NULL, 
    [LastName] NVARCHAR(20) NOT NULL, 
    [TitleBefore] NVARCHAR(10) NOT NULL, 
    [YearOfBirth] INT NOT NULL, 
    [KrpId] INT NOT NULL, 
    [AgeCategory] SMALLINT NULL, 
    [ClubId] INT NULL
)
