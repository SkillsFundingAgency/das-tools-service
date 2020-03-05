CREATE PROCEDURE [dbo].[AddApplicationRoleMapping]
	@RoleId INT,
	@ApplicationId INT
AS
	INSERT INTO [dbo].[ApplicationRole] VALUES(@ApplicationId, @RoleId)
RETURN 0
