CREATE TABLE [dbo].[User]
(
	[Id_User] INT NOT NULL PRIMARY KEY, 
    [Username] VARCHAR(50) NULL,
	[Password] VARCHAR(50) NULL,
	[First_Name] VARCHAR(50) NULL,
	[Last_Name] VARCHAR(50) NULL, 
    [Description] VARCHAR(250) NULL
)
