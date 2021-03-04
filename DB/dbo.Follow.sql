CREATE TABLE [dbo].[Follow]
(
    [Id_Follower] INT NOT NULL, 
    [Id_Followed] INT NOT NULL, 
    CONSTRAINT [FK_Follow_Follower] FOREIGN KEY ([Id_Follower]) REFERENCES [User]([Id_User]), 
    CONSTRAINT [FK_Follow_Followed] FOREIGN KEY ([Id_Followed]) REFERENCES [User]([Id_User]), 

)
