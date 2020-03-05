/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

BEGIN

    IF NOT EXISTS  (SELECT NAME FROM dbo.Role WHERE NAME = 'Admin')

        BEGIN
            INSERT INTO dbo.Role(Name, Description)
            VALUES ('Admin', 'Tool Service Administrators')
            PRINT 'Database roles seeded'
        END

	IF NOT EXISTS (SELECT NAME FROM dbo.Application
					WHERE NAME IN ('Message Service', 'Admin Services'))
		BEGIN
			INSERT INTO dbo.Application (Name, Description, Path, IsExternal, [Public])
			VALUES 
			('Message Service', 'A secure way to send contextless one time messages.', '~/messages', 0, 1),
			('Admin Services', 'Perform basic administrative tasks on tools service', '~/admin', 0, 0)

			PRINT 'Database applications seeded'
		END
END
