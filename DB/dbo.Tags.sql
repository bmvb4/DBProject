CREATE TABLE [dbo].[Tags] (
    [Id_Post] INT NOT NULL,
    [Id_Tag]  INT NOT NULL,
    CONSTRAINT [FK_Tags_Post] FOREIGN KEY ([Id_Post]) REFERENCES [dbo].[Post] ([Id_Post]),
    CONSTRAINT [FK_Tags_Tag] FOREIGN KEY ([Id_Tag]) REFERENCES [dbo].[Tag] ([Id_Tag])
);

