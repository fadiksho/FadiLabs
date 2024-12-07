IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    IF SCHEMA_ID(N'Blog') IS NULL EXEC(N'CREATE SCHEMA [Blog];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE TABLE [Blog].[Posts] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Slug] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [Body] nvarchar(max) NULL,
        [PublishedDate] datetime2 NOT NULL,
        [UpdatedDate] datetime2 NOT NULL,
        [IsPublished] bit NOT NULL,
        [OwndedBy] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Posts] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE TABLE [Blog].[Tags] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Tags] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE TABLE [Blog].[Comments] (
        [Id] uniqueidentifier NOT NULL,
        [Body] nvarchar(max) NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [OwndedBy] uniqueidentifier NOT NULL,
        [PostId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Comments_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [Blog].[Posts] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE TABLE [Blog].[PostTag] (
        [PostsId] uniqueidentifier NOT NULL,
        [TagsId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_PostTag] PRIMARY KEY ([PostsId], [TagsId]),
        CONSTRAINT [FK_PostTag_Posts_PostsId] FOREIGN KEY ([PostsId]) REFERENCES [Blog].[Posts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PostTag_Tags_TagsId] FOREIGN KEY ([TagsId]) REFERENCES [Blog].[Tags] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE INDEX [IX_Comments_PostId] ON [Blog].[Comments] ([PostId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    CREATE INDEX [IX_PostTag_TagsId] ON [Blog].[PostTag] ([TagsId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241203074350_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241203074350_Initial', N'9.0.0');
END;

COMMIT;
GO

