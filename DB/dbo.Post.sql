CREATE TABLE [dbo].[Post] (
    [Id_Post] INT   NOT NULL,
    [Photo]   IMAGE NOT NULL,
    [Id_User] INT   NOT NULL,
    PRIMARY KEY CLUSTERED ([Id_Post] ASC),
    CONSTRAINT [FK_Post_ToTable] FOREIGN KEY ([Id_User]) REFERENCES [dbo].[User] ([Id_User])
);

