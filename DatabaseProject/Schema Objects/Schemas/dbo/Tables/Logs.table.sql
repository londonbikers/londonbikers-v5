CREATE TABLE [dbo].[Logs] (
    [LogID]      INT      IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Type]       TINYINT  NOT NULL,
    [Message]    TEXT     NOT NULL,
    [StackTrace] TEXT     NULL,
    [Context]    TEXT     NULL,
    [When]       DATETIME NOT NULL
);



