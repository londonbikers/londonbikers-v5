/*CREATE TABLE [dbo].[InstantASP_Emails] (
    [EmailID]       INT             IDENTITY (1, 1) NOT NULL,
    [EmailTo]       NVARCHAR (255)  DEFAULT ('') NOT NULL,
    [EmailCC]       NVARCHAR (255)  DEFAULT ('') NOT NULL,
    [EmailBCC]      NTEXT           NULL,
    [EmailFrom]     NVARCHAR (255)  DEFAULT ('') NOT NULL,
    [EmailSubject]  NVARCHAR (1500) DEFAULT ('') NOT NULL,
    [EmailBody]     NTEXT           NULL,
    [EmailPriority] TINYINT         DEFAULT ((1)) NOT NULL,
    [EmailAttempts] TINYINT         DEFAULT ((0)) NOT NULL,
    [DateStamp]     DATETIME        DEFAULT (getdate()) NOT NULL
);*/

