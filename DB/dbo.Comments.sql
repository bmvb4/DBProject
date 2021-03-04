CREATE TABLE [dbo].[Comments]
(
    [Comment] VARCHAR(250) NULL, 
    [Id_User] INT NOT NULL, 
    [Id_Post] INT NOT NULL, 
    CONSTRAINT [FK_Comments_User] FOREIGN KEY ([Id_User]) REFERENCES [User]([Id_User]), 
    CONSTRAINT [FK_Comments_Post] FOREIGN KEY ([Id_Post]) REFERENCES [Post]([Id_Post])
)
