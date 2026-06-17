using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.ServerModel;
using PulseLogicHMR.DalLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using PulseLogicHMR.Utilities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Features;
using PulseLogicHMR.CommonTypes;
// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PulseLogicHMR.Controllers
{

    public class EmployeeViewController : Controller
    {
        private readonly string config = null;
        public EmployeeViewController(IOptions<MyConfiguration> _config)
        {
            config = _config.Value.Connectionstring;

        }

        public IActionResult UserProfile()
        {
            try
            {
                ViewData["ConnectionString"] = config;
                return View("UserProfile");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public IActionResult ChangePassword()
        {
            try
            {
                ViewData["ConnectionString"] = config;
                return View("ChangePassword");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        

        public IActionResult ProfileMain()
        {
            try
            {
                ViewData["ConnectionString"] = config;
                return View("ProfileMain");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult ChangeProfile()
        {
            try
            {
                ViewData["ConnectionString"] = config;
                return View("ChangeProfile");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        
    }
}


