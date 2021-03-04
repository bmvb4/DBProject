CREATE TABLE [dbo].[Likes]
(
    [Id_User] INT NOT NULL, 
    [Id_Post] INT NOT NULL, 
    CONSTRAINT [FK_Likes_User] FOREIGN KEY ([Id_User]) REFERENCES [User]([Id_User]), 
    CONSTRAINT [FK_Likes_Post] FOREIGN KEY ([Id_Post]) REFERENCES [Post]([Id_Post])

)
