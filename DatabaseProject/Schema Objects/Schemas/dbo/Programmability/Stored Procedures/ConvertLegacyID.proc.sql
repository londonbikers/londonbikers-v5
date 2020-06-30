
CREATE Procedure dbo.ConvertLegacyID
(
	@DomainObjectType varchar(50),
	@UID uniqueidentifier
)
AS
	IF (@DomainObjectType = 'Document')
	BEGIN
		SELECT ID FROM apollo_content WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'DocumentImage')
	BEGIN
		SELECT i.ID, m.ContentID FROM apollo_images i INNER JOIN apollo_content_images m on m.ImageID = i.ID WHERE i.f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'Gallery' OR @DomainObjectType = 'GallerySlideshow')
	BEGIN
		SELECT ID FROM apollo_galleries WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'GalleryImage' OR @DomainObjectType = 'GalleryImageHandler')
	BEGIN
		SELECT ID FROM GalleryImages WHERE [Uid] = @UID
	END
	ELSE IF (@DomainObjectType = 'GalleryCategory')
	BEGIN
		SELECT ID FROM apollo_gallery_categories WHERE f_uid = @UID
	END
	ELSE IF (@DomainObjectType = 'DirectoryCategory')
	BEGIN
		SELECT ID FROM DirectoryCategories WHERE [UID] = @UID
	END
	ELSE IF (@DomainObjectType = 'DirectoryItem')
	BEGIN
		SELECT ID FROM DirectoryItems WHERE [UID] = @UID
	END
