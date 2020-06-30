------------------------------------------------------------------------
CREATE VIEW [dbo].[TopContentByViews]
AS
SELECT     ID, f_title AS Title, Type AS RowType, 'Document' AS ContentType, [Views]
FROM         dbo.apollo_content
WHERE     ([Views] > 0)
UNION ALL
SELECT     ID, f_name AS Title, Type AS RowType, 'Document Image' AS ContentType, [Views]
FROM         dbo.apollo_images
WHERE     ([Views] > 0)
UNION ALL
SELECT     ID, [Name] AS Title, 0 AS RowType, 'Gallery Image' AS ContentType, [Views]
FROM         dbo.GalleryImages
WHERE     ([Views] > 0)
UNION ALL
SELECT     TOP (100) PERCENT ID, Title, 0 AS RowType, 'Directory Item' AS ContentType, [Views]
FROM         dbo.DirectoryItems
WHERE     ([Views] > 0)
ORDER BY [Views] DESC
