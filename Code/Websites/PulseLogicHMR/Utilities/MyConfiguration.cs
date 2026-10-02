using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//try to move this to some other namespace later on.-- sud: 8June'18
namespace PulseLogicHMR.Core.Configuration
{
    public class MyConfiguration
    {
        public string Connectionstring { get; set; }
        public string ConnectionStringAdmin { get; set; }
        public string ConnectionStringPACSServer { get; set; }
        public int CacheExpirationMinutes { get; set; }
        public string FileStorageRelativeLocation { get; set; }
        public bool highlightAbnormalLabResult { get; set; }
        public bool RealTimeRemoteSyncEnabled { get; set; }
        public string ApplicationVersionNum { get; set; }
        public bool IsAuditEnable { get; set; }
        public string LISDataBaseUrl { get; set; }
        public GoogleDriveConfiguration GoogleDriveFileUpload { get; set; }
        public JWTTokenConfiguration JwtTokenConfig { get; set; }
        public bool RealTimeSSFClaimBooking { get; set; }
        public ABDMConfiguration ABDMConfig { get; set; }
    }
    public class GoogleDriveConfiguration
    {
        public string ServiceAccountKey { get; set; }
        public string LoggerFilePath { get; set; }
        public string UploadFileBasePath { get; set; }
        public string FileUrlCommon { get; set; }
    }

    public class JWTTokenConfiguration
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public string JwtValidMinutes { get; set; }
    }

    public class ABDMConfiguration
    {
        public string BaseUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool UseMockData { get; set; }
    }
}
