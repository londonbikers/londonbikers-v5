IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'if_sp_AddPostAttachment')
	BEGIN
		PRINT 'Dropping Procedure if_sp_AddPostAttachment'
		DROP Procedure if_sp_AddPostAttachment
	END
GO

PRINT 'Creating Procedure if_sp_AddPostAttachment'
GO

CREATE Procedure if_sp_AddPostAttachment
	@MessageID int,
	@FileName varchar(255),
	@MimeType varchar(255),
	@ContentLength int,
	@PrivateMessage bit
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

INSERT INTO 
	InstantForum_MessageAttachments 
	(
		MessageID, 
		[FileName], 
		ContentLength, 
		MimeType, 
		PrivateMessage
	)
	VALUES 
	(
		@MessageID, 
		@FileName, 
		@ContentLength,
		@MimeType, 
		@PrivateMessage
	)
	
RETURN
GO

GRANT EXEC ON if_sp_AddPostAttachment TO PUBLIC
GO