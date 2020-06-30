BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.DirectoryCategories ADD
	ParentCategoryUID uniqueidentifier NULL
GO
COMMIT

---------------------------------------------

BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
ALTER TABLE dbo.DirectoryItems ADD
	TelephoneNumber varchar(20) NULL
GO
COMMIT 

-----------------------------------------------

update DirectoryItems set submiter = '34450309-9B9C-4C38-A5E1-C867E99C37D5' where submiter = 'CC1EE292-7225-4EE9-AA26-8BD65DAE7CB8'
update DirectoryItems set submiter = '5C28A7B5-C3D7-4416-9279-EDE1218554DF' where submiter = 'E74A6699-722A-44A1-9A35-B2DC4D5E4CB3'