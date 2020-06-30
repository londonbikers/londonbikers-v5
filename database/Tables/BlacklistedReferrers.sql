IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'BlacklistedReferrers')
	BEGIN
		PRINT 'Dropping Table BlacklistedReferrers'
		DROP Table BlacklistedReferrers
	END
GO

/******************************************************************************
**		Name: BlacklistedReferrers
**		Desc: Stores a simple list of the blacklisted referrers.
**
**		Auth: Jay Adair
**		Date: 16.11.05
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

PRINT 'Creating Table BlacklistedReferrers'
GO

CREATE TABLE [dbo].[BlacklistedReferrers] (
	[Url] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	CONSTRAINT [PK_BlacklistedReferrers] PRIMARY KEY  CLUSTERED 
	(
		[Url]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

GRANT SELECT ON [dbo].BlacklistedReferrers TO PUBLIC
GO