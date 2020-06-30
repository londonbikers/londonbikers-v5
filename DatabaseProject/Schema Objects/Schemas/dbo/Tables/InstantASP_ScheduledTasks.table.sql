/*CREATE TABLE [dbo].[InstantASP_ScheduledTasks] (
    [TaskID]           INT            IDENTITY (1, 1) NOT NULL,
    [TaskName]         NVARCHAR (255) DEFAULT ('') NOT NULL,
    [TaskDescription]  NVARCHAR (500) DEFAULT ('') NOT NULL,
    [TaskFileName]     NVARCHAR (255) DEFAULT ('') NOT NULL,
    [TaskNextRunDate]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [TaskLastRunDate]  DATETIME       NULL,
    [TaskInterval]     SMALLINT       DEFAULT ((0)) NOT NULL,
    [TaskIntervalType] TINYINT        DEFAULT ((1)) NOT NULL,
    [TaskEnabled]      BIT            DEFAULT ((0)) NOT NULL,
    [TaskGuid]         NVARCHAR (75)  DEFAULT ('') NOT NULL,
    [DateStamp]        DATETIME       DEFAULT (getdate()) NOT NULL
);*/

