using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PulseLogicHMR.Core.Configuration;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Security;
using System.Collections.Generic;
using PulseLogicHMR.Utilities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PulseLogicHMR.Controllers.HelpDesk
{
    public class HelpdeskViewController : Controller
    {
        private readonly string connString = null;
        public HelpdeskViewController(IOptions<MyConfiguration> _config)
        {
            connString = _config.Value.Connectionstring;

        }
        public IActionResult GetView(string urlFullPath, string viewPath)
        {
            //Get Valid Route List for logged in user
            List<PulseLogicRoute> validRouteList = HttpContext.Session.Get<List<PulseLogicRoute>>("validRouteList");
            if (validRouteList != null || validRouteList.Count > 0)
            {
                List<PulseLogicRoute> validRoutes = validRouteList.Where(a => a.UrlFullPath == urlFullPath).ToList();
                if (validRoutes != null && validRoutes.Count > 0)
                {
                    //Return view for valid user
                    return View(viewPath);
                }
                else
                {
                    //Return content with message for unauthorized user
                    return Content("<div>Page Not Found</div><router-outlet></router-outlet>");
                }
            }
            else
            {
                //Return content with message for unauthorized user
                return Content("<div>Page Not Found</div><router-outlet></router-outlet>");
            }
        }
        public IActionResult EmployeeInformation()
        {
            try
            {
                ViewData["ConnectionString"] = connString;
                return this.GetView("Helpdesk/EmployeeInformation", "EmployeeInformation");                
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult BedInformation()
        {
            try
            {
                ViewData["ConnectionString"] = connString;
                return this.GetView("Helpdesk/BedInformation", "BedInformation");                
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult WardInformation()
        {
            try
            {
                ViewData["ConnectionString"] = connString;
                return this.GetView("Helpdesk/WardInformation", "WardInformation");                
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult Helpdesk()
        {
            try
            {                
                ViewData["ConnectionString"] = connString;
                return this.GetView("Helpdesk", "Helpdesk");                                
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
