/*EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'OVH0001';*/


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'IIS APPPOOL\londonbikers-v5';


GO
EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'NT AUTHORITY\NETWORK SERVICE';


GO
/*EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'KS24238\root';*/


GO
/*EXECUTE sp_addrolemember @rolename = N'db_accessadmin', @membername = N'IIS APPPOOL\londonbikers-v5';*/


GO
/*EXECUTE sp_addrolemember @rolename = N'db_datareader', @membername = N'IIS APPPOOL\londonbikers-v5';*/


GO
/*EXECUTE sp_addrolemember @rolename = N'db_datawriter', @membername = N'IIS APPPOOL\londonbikers-v5';*/

EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'IIS APPPOOL\londonbikers-v5-forum';


GO
/*EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'IIS APPPOOL\londonbikers-forum-2010';*/


GO
/*EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'IIS APPPOOL\londonbikers-v5.1';*/


GO
/*EXECUTE sp_addrolemember @rolename = N'db_owner', @membername = N'IIS APPPOOL\londonbikers-v4';*/

