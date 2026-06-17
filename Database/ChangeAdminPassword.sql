-- This script updates the default 'admin' password to 'P@ssw0rd!2024'
-- Execute this script on the PulseLogicAdmin database after restoration.

USE [DEV_PulseLogicHMR_INT];
GO

UPDATE [dbo].[RBAC_User]
SET [Password] = 'pHJiX1WIv6nTpYJdfmpTZA==',
    [NeedsPasswordUpdate] = 1,
    [ModifiedOn] = GETDATE()
WHERE [UserName] = 'admin';
GO
