IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'StatGalleryImageCount')
	BEGIN
		PRINT 'Dropping Procedure StatGalleryImageCount'
		DROP Procedure StatGalleryImageCount
	END
GO

PRINT 'Creating Procedure StatGalleryImageCount'
GO

CREATE Procedure StatGalleryImageCount
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
**     ----------							-----------
**
**		Auth: 
**		Date: 
*******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:				Description:
**		--------		--------				-------------------------------------------
**    
*******************************************************************************/

SELECT count(0) FROM GalleryImages

GO

GRANT EXEC ON StatGalleryImageCount TO PUBLIC
GO