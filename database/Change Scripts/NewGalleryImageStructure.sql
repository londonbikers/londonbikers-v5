DECLARE @ImageUID uniqueidentifier
DECLARE @GalleryUID uniqueidentifier
DECLARE c1 CURSOR FOR
	SELECT 
		f_gallery_uid,
		f_image_uid 
		FROM 
		apollo_gallery_image_relations
OPEN c1
FETCH NEXT FROM c1
INTO @GalleryUID, @ImageUID
WHILE @@FETCH_STATUS = 0
BEGIN

	UPDATE 
		GalleryImages
		SET
		GalleryUID = @GalleryUID
		WHERE
		Uid = @ImageUID
	FETCH NEXT FROM c1
	INTO @GalleryUID, @ImageUID 

END
CLOSE c1
DEALLOCATE c1

DELETE FROM
	GalleryImages
	WHERE
	GalleryUID is null

DROP TABLE apollo_gallery_image_relations
DROP PROCEDURE CreateImageGalleryRelationship