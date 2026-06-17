EXEC xp_cmdshell 'mkdir "C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\DEV_PulseLogicHMR_INT_FS"';
GO

USE [master];
GO

RESTORE DATABASE [DEV_PulseLogicHMR_INT]
FROM DISK = N'D:\App and Hardware Project\App\PulseLogicHMR_v2.0\Database\2. EMR-Db\PulseLogicInternationalDB\extracted\Dev_PulseLogicHMR_INT1.bak'
WITH FILE = 1,
MOVE N'PulseLogic_MNK_FINAL'       TO N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\DEV_PulseLogicHMR_INT.mdf',
MOVE N'PulseLogic_MNK_FINAL_log'   TO N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\DEV_PulseLogicHMR_INT_log.ldf',
MOVE N'PulseLogic_PatientFiles'     TO N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\DEV_PulseLogicHMR_INT_FS',
NOUNLOAD, REPLACE, RECOVERY, STATS = 10;
GO

PRINT 'DEV_PulseLogicHMR_INT restore complete.';
GO
