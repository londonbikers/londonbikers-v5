IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'StatMostPopularImage')
	BEGIN
		PRINT 'Dropping Procedure StatMostPopularImage'
		DROP Procedure StatMostPopularImage
	END
GO

PRINT 'Creating Procedure StatMostPopularImage'
GO

CREATE Procedure StatMostPopularImage
AS

/******************************************************************************
**		File: 
**		Name: Stored_Procedure_Name
**		Desc: 
**
**		This template can be customized:
**              
**		Return values:
** 
**		Called by:   
**              
**		Parameters:
**		Input							Output
**      ----------						-----------
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------	--------			---------------------------------------
**    
*******************************************************************************/

SELECT 
	TOP 1 
	Uid 
	FROM GalleryImages 
	GROUP BY 
	Uid 
	ORDER BY 
	MAX(Views) DESC

GO

GRANT EXEC ON StatMostPopularImage TO PUBLIC
GO