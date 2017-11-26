CREATE TABLE [dbo].[Clubs]
(
	[ClubID] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(30) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [Url] NVARCHAR(50) NOT NULL
)


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

ALTER TABLE Players ADD CONSTRAINT [FK_Players_Clubs] FOREIGN KEY (ClubId) REFERENCES [Clubs]([ClubID]);
