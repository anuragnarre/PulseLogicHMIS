using Audit.SqlServer.Providers;
using PulseLogicHMR.CommonTypes;
using PulseLogicHMR.Controllers.Settings.DTO;
using PulseLogicHMR.Core.Caching;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.DependencyInjection;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.Services;
using PulseLogicHMR.Services.Billing;
using PulseLogicHMR.Services.ClaimManagement;
using PulseLogicHMR.Services.Dispensary;
using PulseLogicHMR.Services.DispensaryTransfer;
using PulseLogicHMR.Services.IMU;
using PulseLogicHMR.Services.Inventory.InventoryDonation;
using PulseLogicHMR.Services.LIS;
using PulseLogicHMR.Services.Maternity;
using PulseLogicHMR.Services.Medicare;
using PulseLogicHMR.Services.Pharmacy.Mapper.PurchaseOrder;
using PulseLogicHMR.Services.Pharmacy.PharmacyPO;
using PulseLogicHMR.Services.Pharmacy.Rack;
using PulseLogicHMR.Services.Pharmacy.SupplierLedger;
using PulseLogicHMR.Services.ProcessConfirmation;
using PulseLogicHMR.Services.QueueManagement;
using PulseLogicHMR.Services.SSF;
using PulseLogicHMR.Services.Utilities;
using PulseLogicHMR.Services.Vaccination;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace PulseLogicHMR
{
    public class Startup
    {

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            CurrentEnvironment = env;
            //check audit is enable or disable             
            if ((!string.IsNullOrEmpty(Configuration["IsAuditEnable"])) && Convert.ToBoolean(Configuration["IsAuditEnable"]) == true)
            {
                string adminConstr = Configuration["ConnectionStringAdmin"];
                SqlConnectionStringBuilder conBuilderObj = new SqlConnectionStringBuilder(adminConstr);
                string encPwd = conBuilderObj.Password;
                if (!string.IsNullOrEmpty(encPwd))  //check is it encrypted or not
                {
                    string decPwd = DecryptPassword(encPwd);
                    conBuilderObj.Password = decPwd;
                }
                Audit.Core.Configuration.DataProvider = new SqlDataProvider()
                {
                    ConnectionString = conBuilderObj.ConnectionString,
                    Schema = "dbo",
                    TableName = "PulseLogicAudit",
                    IdColumnName = "AuditId",
                    JsonColumnName = "Data",
                    LastUpdatedDateColumnName = "LastUpdatedDate"
                };
            }


        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //start--for rbac-testing--developerr--2march-2017
            //services.AddSession();

            // Adds a default in-memory implementation of IDistributedCache.
            //services.AddDistributedMemoryCache();//removed after using sqlserver-distributed cache-18apr'17-developer

            services.AddSession(options =>
            {
                //IMPORTANT-- remove the hardcoded value 20 from below
                //keep short timeout like max 2-3 hours, 
                //we've to redirect to login once the session expires.
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;

            });
            //end--for rbac-testing--developerr

            //Krishna, 19thMay'23, Moved all the interfaces and services that were registered here into an extension method, Do not registered them in Startup file itself,
            //instead add them in that extension method(used just below this comment)
            services.AddPulseLogicServices(Configuration);

            services.AddAutoMapper(typeof(MappingProfile));
            services.AddAutoMapper(typeof(PurchaseOrderMappingProfile));

            // Add framework services.
            services.AddDetection();
            services.AddOptions();

            services.Configure<MyConfiguration>(Configuration);

            //Krishna, 19thMay'23, Moved the registration of Swagger and JWT to an extension method inside ConfigureServices class.
            services.AddSwaggerAndJwtServices(Configuration);

            //start: sud-9Jan'19 for pwd encryption testing

            //For PulseLogicHMR Database connectionstring..
            //reads connectionstring
            string connStr = Configuration["Connectionstring"];
            //this inbuilt class maps connString to separate properties                                                  
            SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder(connStr);
            string encPassword = connStringBuilder.Password;
            //if password is present, it must be in encrypted form., only then go for decryption, else keep it as it is.
            if (!string.IsNullOrEmpty(encPassword))
            {
                //decrypt the password and re-assign the values to actual Connstring
                string decrypted = DecryptPassword(encPassword);//this calls our internal function of RBAC
                connStringBuilder.Password = decrypted;
                Configuration["Connectionstring"] = connStringBuilder.ToString();
            }

            //For PulseLogicAdmin Database connectionstring.
            string connStrAdmin = Configuration["ConnectionStringAdmin"];
            SqlConnectionStringBuilder connStringBuilder2 = new SqlConnectionStringBuilder(connStrAdmin);
            string encPwd_Admin = connStringBuilder2.Password;
            //if password is present, only then go for decryption, else keep it as it is.
            if (!string.IsNullOrEmpty(encPwd_Admin))
            {
                string decPwd_Admin = DecryptPassword(encPwd_Admin);//this calls our internal function of RBAC
                connStringBuilder2.Password = decPwd_Admin;
                Configuration["ConnectionStringAdmin"] = connStringBuilder2.ToString();
            }

            //end: sud-9Jan'19 for pwd encryption testing


            services.AddMvc(options => options.EnableEndpointRouting = false)
                        .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver())
                        .AddRazorRuntimeCompilation();

            //services.AddMvc().AddApplicationPart(typeof(LoginViewModel).Assembly);
            //start: using service configuration for caching class.--developer 1march'17
            //once we've added appsetting.json into the Configuration, it's accessible easily using the key.
            string connString = Configuration["Connectionstring"];
            int cacheExpMins = Convert.ToInt32(Configuration["CacheExpirationMinutes"]);
            //add cache as singleton since there should be one global object of it.. 
            services.AddSingleton<PulseLogicCache>(new PulseLogicCache(connString, cacheExpMins));
            
            // Registering native IMemoryCache for modern distributed/memory caching implementations
            services.AddMemoryCache();
            services.AddSingleton<PulseLogicHMR.Services.Utilities.ICacheService, PulseLogicHMR.Services.Utilities.MemoryCacheService>();
            
            //end: using service configuration for caching class.

            //used in LabReportExport to differntiate abnormal values based on this parameter
            bool highlightAbnormalLabResult = Convert.ToBoolean(Configuration["highlightAbnormalLabResult"]);

            //AuditTrail Enable and Disable 
            string auditValueStr = Configuration["IsAuditEnable"];
            if (!string.IsNullOrEmpty(auditValueStr))
            {
                bool IsAuditEnable = Convert.ToBoolean(auditValueStr);
                if (IsAuditEnable == false)
                {
                }
            }
            else
            {
            }

            ////for distributed sqlserver caching --added: 18Apr2017-developer
            //services.AddDistributedSqlServerCache(options =>
            //{
            //    options.ConnectionString = connString;
            //    options.SchemaName = "dbo";
            //    options.TableName = "CORE_DistributedCache"; // this has to be same what we gave while running the command.
            //});

            //add RBAC also as a singleton service.          
            services.AddSingleton<RBAC>(new RBAC(connString, cacheExpMins));
            ///add nepali date as a singleton for one time initialization.
            services.AddSingleton<NepaliDate>(new NepaliDate(connString));

            //add FileUploader as a singleton as well
            string storagePath = CurrentEnvironment.WebRootPath + "\\" + Configuration["FileStorageRelativeLocation"];
            services.AddSingleton<FileUploader>(new FileUploader(storagePath));


            services.Configure((Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.MvcRazorRuntimeCompilationOptions options) =>
                {
                    options.AdditionalReferencePaths.Add(typeof(MasterDbContext).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(CountryModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(CountrySubDivisionModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(ICD10CodeModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(EmployeeModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(ServiceDepartmentModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(LoginViewModel).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(PulseLogicRoute).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(PulseLogicCache).Assembly.Location);
                    options.AdditionalReferencePaths.Add(typeof(Wangkanai.Detection.Services.IDetectionService).Assembly.Location);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDetection();
            app.UseMiddleware<PulseLogicHMR.Utilities.ExceptionHandlingMiddleware>();
            app.UseMiddleware<RewindMiddleWare>();
            //start--for rbac-testing--developerr--2march-2017
            app.UseSession();

            //end--for rbac-testing--developerr


            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(name: "AngularSPA1", template: "PulseLogicApp/dist/PulseLogicApp", defaults: new { controller = "Home", action = "Index" });
                    routes.MapRoute(name: "AngularSPA2", template: "PulseLogicApp/dist/PulseLogicApp/", defaults: new { controller = "Home", action = "Index" });
                    routes.MapRoute("DefaultRoute", "{controller}/{action}");
                    routes.MapRoute(name: "Default", template: "{controller}/{action}", defaults: new { controller = "Account", action = "Login" });
                }
                );
            app.UseFileServer();

            // set a home page
            DefaultFilesOptions defaultoptions = new DefaultFilesOptions();
            defaultoptions.DefaultFileNames.Clear();
            defaultoptions.DefaultFileNames.Add("UI/Main.html");
            app.UseDefaultFiles(defaultoptions);
            //app.UseMiddleware<>
            app.UseStaticFiles();
            // this will serve up node_modules
            // this code is bad here i am saying add the mode_modules
            // with wwwroot. This i did because when a page runs 
            // inside wwwroot he has no way to access folders outside
            // the wwwroot. Putting this complete folder inside
            // wwwroot would be a kill. So later 
            // we need to write a grunt task which will copy the necessaayr files
            // to wwwroot. The task woul sit here

            string isDevEnv = Configuration["environment:isdevelopment"];


            if (bool.TryParse(isDevEnv, out bool isDev) && isDev)
            {
                //Use Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PulseLogicHMR-APIs-v1");
                });
            }
        }


        //start: sud-9Jan'19-- for ConnectionString encryption/decryption

        //use existing decrypt method from RBAC.
        private string DecryptPassword(string encryptedPwd)
        {
            string retVal = PulseLogicHMR.Security.RBAC.DecryptPassword(encryptedPwd);
            return retVal;
        }

        //end: sud-9Jan'19-- for ConnectionString encryption/decryption

    }
}

