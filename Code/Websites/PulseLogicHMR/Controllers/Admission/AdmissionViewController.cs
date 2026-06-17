using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.Security;
using PulseLogicHMR.Utilities;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PulseLogicHMR.Controllers
{
    public class AdmissionViewController : Controller
    {
        private readonly string config = null;
        public AdmissionViewController(IOptions<MyConfiguration> _config)
        {
            config = _config.Value.Connectionstring;

        }
        // GET: /<controller>/
        [PulseLogicViewFilter("adt-createadmission-view")]
        public IActionResult AdmissionCreate()
        {
            try
            {
                ViewData["ConnectionString"] = config;
                return View("CreateAdmission");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        [PulseLogicViewFilter("adt-admissionsearchpatient-view")]
        public IActionResult AdmissionSearchPatient()
        {
            try
            {
                return View("AdmissionSearchPatient");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        [PulseLogicViewFilter("adt-admittedlist-view")]
        public IActionResult AdmittedList()
        {
            try
            {
                return View("AdmittedList");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        [PulseLogicViewFilter("adt-dischargedlist-view")]
        public IActionResult DischargedList()
        {
            try
            {
                return View("DischargedList");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        [PulseLogicViewFilter("adt-view")]
        public IActionResult Admission()
        {
            try
            {
                return View("Admission");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
