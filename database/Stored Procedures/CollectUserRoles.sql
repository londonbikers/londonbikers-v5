IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'CollectUserRoles')
	BEGIN
		PRINT 'Dropping Procedure CollectUserRoles'
		DROP Procedure CollectUserRoles
	END
GO

PRINT 'Creating Procedure CollectUserRoles'
GO

CREATE Procedure dbo.CollectUserRoles
AS

/******************************************************************************
**		Name: CollectUserRoles
**		Desc: 
**
**		Auth: Jay Adair
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SET NOCOUNT ON
SELECT
	f_key as [ID],
	f_role as [Name]
	FROM
	apollo_roles
GO

GRANT EXEC ON dbo.CollectUserRoles TO PUBLIC
GO