/*CREATE TABLE [dbo].[InstantForum_PrivateMessages] (
    [PrivateMessageID]     INT            IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AuthorID]             INT            NULL,
    [RecipientID]          CHAR (10)      NULL,
    [Title]                NVARCHAR (255) NULL,
    [Message]              NTEXT          NULL,
    [DateStamp]            DATETIME       NULL,
    [ReadFlag]             BIT            NULL,
    [ReadReceipt]          BIT            NOT NULL,
    [MessageIconName]      NVARCHAR (200) NOT NULL,
    [DeletedFromInBox]     BIT            NULL,
    [DeletedFromSentItems] BIT            NULL,
    [FolderID]             INT            /*DEFAULT ((0))*/ NOT NULL,
    [Description]          NVARCHAR (300) /*DEFAULT ('')*/ NOT NULL,
    [HasAttachments]       BIT            /*DEFAULT ((0))*/ NOT NULL
);*/

