﻿IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
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
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    IF SCHEMA_ID(N'User') IS NULL EXEC(N'CREATE SCHEMA [User];');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE TABLE [User].[LabRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [LabsPermissions] int NOT NULL,
        CONSTRAINT [PK_LabRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE TABLE [User].[LabUsers] (
        [Id] uniqueidentifier NOT NULL,
        [Auth0UserId] nvarchar(50) NOT NULL,
        [DisplayName] nvarchar(max) NULL,
        [Email] nvarchar(50) NOT NULL,
        [EmailVerified] bit NOT NULL,
        [ProfilePictureUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_LabUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE TABLE [User].[LabRoleLabUser] (
        [LabRolesId] uniqueidentifier NOT NULL,
        [LabUsersId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_LabRoleLabUser] PRIMARY KEY ([LabRolesId], [LabUsersId]),
        CONSTRAINT [FK_LabRoleLabUser_LabRoles_LabRolesId] FOREIGN KEY ([LabRolesId]) REFERENCES [User].[LabRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_LabRoleLabUser_LabUsers_LabUsersId] FOREIGN KEY ([LabUsersId]) REFERENCES [User].[LabUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE INDEX [IX_LabRoleLabUser_LabUsersId] ON [User].[LabRoleLabUser] ([LabUsersId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_LabRoles_Name] ON [User].[LabRoles] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_LabUsers_Auth0UserId] ON [User].[LabUsers] ([Auth0UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20241207211046_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20241207211046_Initial', N'9.0.0');
END;

COMMIT;
GO

