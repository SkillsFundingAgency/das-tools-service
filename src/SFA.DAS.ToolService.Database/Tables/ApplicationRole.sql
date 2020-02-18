CREATE TABLE [dbo].[ApplicationRole]
(
	[Id] INT IDENTITY PRIMARY KEY,
    [ApplicationId] INT NOT NULL,
    RoleId INT NOT NULL, 
    CONSTRAINT [FK_ApplicationRole_Application] FOREIGN KEY (ApplicationId) REFERENCES [Application]([Id]),
    CONSTRAINT [FK_ApplicationRole_Role] FOREIGN KEY (RoleId) REFERENCES [Role]([Id]),
    CONSTRAINT [UQ_ApplicationRole] UNIQUE(ApplicationId, RoleId)
)
