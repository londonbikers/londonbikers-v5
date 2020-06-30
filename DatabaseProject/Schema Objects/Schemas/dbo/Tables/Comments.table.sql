CREATE TABLE [dbo].[Comments] (
    [ID]                   BIGINT           IDENTITY (1, 1) NOT NULL,
    [AuthorID]             UNIQUEIDENTIFIER NOT NULL,
    [Created]              DATETIME         NOT NULL,
    [Comment]              NTEXT            NOT NULL,
    [Status]               TINYINT          NOT NULL,
    [OwnerID]              BIGINT           NOT NULL,
    [OwnerType]            TINYINT          NOT NULL,
    [ReportStatus]         TINYINT          NOT NULL,
    [ReceiveNotification]  BIT              NOT NULL,
    [RequiresNotification] BIT              NOT NULL
);



