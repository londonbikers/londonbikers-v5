
-------------------------------------------------------------------

CREATE PROCEDURE [dbo].[DeleteGalleryCategory]
	@ID bigint
AS
	DELETE FROM
		apollo_gallery_categories
		WHERE
		ID = @ID
		
	-- this should surfice, Apollo will ensure all relations underneath are severed before hand.
		
	DELETE FROM
		apollo_gallery_category_gallery_relations
		WHERE
		CategoryID = @ID
