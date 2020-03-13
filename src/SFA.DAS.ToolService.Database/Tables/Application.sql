CREATE TABLE [dbo].[Application]
(
	[Id] INT IDENTITY PRIMARY KEY, 
    [Name] VARCHAR(50) NOT NULL, 
    [Description] VARCHAR(100) NOT NULL, 
    [Path] VARCHAR(50) NOT NULL, 
    [IsExternal] INT NOT NULL DEFAULT 0,
    [Public] INT NOT NULL DEFAULT 0, 
    [Admin] INT NOT NULL DEFAULT 0
)
