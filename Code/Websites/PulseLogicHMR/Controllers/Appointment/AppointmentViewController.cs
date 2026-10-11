using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using Microsoft.AspNetCore.Http;
using PulseLogicHMR.Utilities;
using PulseLogicHMR.Security;

//review:25Jan'17-developer: check if ViewData["ConnectionString"] is needed or not in respective cshtml, remove from here if not needed.
namespace PulseLogicHMR.Controllers
{
    public class AppointmentViewController : Controller
    {
        private readonly string connString = null;
        public AppointmentViewController(IOptions<MyConfiguration> _config)
        {
            connString = _config.Value.Connectionstring;

        }      
        [PulseLogicViewFilter("appointment-visit-view")]
        public IActionResult Visit()
        {
                ViewData["ConnectionString"] = connString;
                return View("Visit");              

        }
        [PulseLogicViewFilter("appointment-createappointment-view")]
        public IActionResult CreateAppointment()
        {
                ViewData["ConnectionString"] = connString;
                return View("CreateAppointment");                

        }
        public IActionResult CreateAdmission()
        {
                ViewData["ConnectionString"] = connString;                
                return View("CreateAdmission");

        }
        [PulseLogicViewFilter("appointment-view")]
        public IActionResult Appointment()
        {
                //RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");              
                //List<PulseLogicRoute> childRoutes = RBAC.GetChildRoutesForUser(currentUser.UserId, urlFullPath: "Appointment");
                //ViewData["validroutes"] = childRoutes;                
                ViewData["ConnectionString"] = connString;
                return View("Appointment");                

        }
        [PulseLogicViewFilter("appointment-listappointment-view")]
        public IActionResult ListAppointment()
        {
                ViewData["ConnectionString"] = connString;
                return View("ListAppointment");                

        }
        [PulseLogicViewFilter("appointment-listvisit-view")]
        public IActionResult ListVisit()
        {
                ViewData["ConnectionString"] = connString;
                return View("ListVisit");                

        }

        //removed viewfilter attribute since this is being loaded from shared-module and it shouldn't require extra permissions.
        //[PulseLogicViewFilter("appointment-printsticker-view")]
        public IActionResult PrintSticker()
        {
                ViewData["ConnectionString"] = connString;
                return View("PrintSticker");                

        }
        [PulseLogicViewFilter("appointment-patientsearch-view")]
        public IActionResult SearchPatient()
        {
                ViewData["ConnectionString"] = connString;
                return View("SearchPatient");                

        }


    }
}
