CREATE DEFAULT [dbo].[nulldate]
    AS '1/1/2000';


GO
EXECUTE sp_bindefault @defname = N'[dbo].[nulldate]', @objname = N'[dbo].[apollo_content].[f_publish_date]';

