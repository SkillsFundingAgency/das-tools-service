CREATE TABLE [dbo].Role
(
	[Id] INT IDENTITY PRIMARY KEY,
    [ExternalId] VARCHAR(50),
    [Name] VARCHAR(50) NOT NULL,
    [Description] VARCHAR(150),
    CONSTRAINT [UQ_Role] UNIQUE(ExternalId)

)


