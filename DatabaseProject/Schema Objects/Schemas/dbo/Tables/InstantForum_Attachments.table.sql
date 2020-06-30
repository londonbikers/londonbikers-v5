/*CREATE TABLE [dbo].[InstantForum_Attachments] (
    [AttachmentID]   INT            IDENTITY (1, 1) NOT NULL,
    [UserID]         INT            /*DEFAULT ((0))*/ NOT NULL,
    [Filename]       NVARCHAR (255) /*DEFAULT ('')*/ NOT NULL,
    [AttachmentBLOB] IMAGE          NULL,
    [ContentType]    NVARCHAR (100) /*DEFAULT ('')*/ NOT NULL,
    [ContentLength]  INT            /*DEFAULT ((0))*/ NOT NULL,
    [Views]          INT            /*DEFAULT ((0))*/ NOT NULL,
    [Hash]           NVARCHAR (50)  /*DEFAULT ('')*/ NOT NULL,
    [DateStamp]      DATETIME       /*DEFAULT (getdate())*/ NOT NULL,
    [Expires]        DATETIME       /*DEFAULT (getdate())*/ NULL
);*/

