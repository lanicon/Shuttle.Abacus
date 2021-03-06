﻿CREATE TABLE [dbo].[KeyStore] (
    [Key] VARCHAR (160)    NOT NULL,
    [Id]  UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_KeyStore] PRIMARY KEY CLUSTERED ([Key] ASC) WITH (FILLFACTOR = 50)
);


GO
CREATE NONCLUSTERED INDEX [IX_KeyStore]
    ON [dbo].[KeyStore]([Id] ASC);

