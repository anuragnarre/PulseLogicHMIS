using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using PulseLogicHMR.Security;
using PulseLogicHMR.Utilities;
using Microsoft.Extensions.Options;
using PulseLogicHMR.Core.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace PulseLogicHMR.Controllers
{
    public class HomeController : Controller
    {

        private readonly string connString = null;
        public HomeController(IOptions<MyConfiguration> _config)
        {
            connString = _config.Value.Connectionstring;

        }

        public IActionResult Index()
        {
            try
            {
                // Check if a valid server-side session exists.
                // If the server was restarted, the in-memory session is wiped even though
                // the browser still has the session cookie — this guard prevents Angular
                // from bootstrapping with a null/empty JWT token which causes infinite redirect loops.
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                if (currentUser == null || currentUser.UserId == 0)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Token is passed via TempData set by AccountController.Login (POST)
                // Index.cshtml reads TempData["loginJwtToken"] directly — no ViewData needed.
                return View();
            }
            catch (Exception ex)
            {
                //Redirect to Login page If user is not login                           
                return RedirectToAction("Login", "Account");
            }
        }
        //move it out of patientcontroller to Maincontroller or something..
        public IActionResult AppMain()
        {
            try
            {
                RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
                ViewData["currentuser"] = currentUser;
                //getting only the root level routes for this view.//set getHierarchy = true for AppMain (special condition.)
                ViewData["validroutes"] = RBAC.GetRoutesForUser(currentUser.UserId, getHiearrchy: true);
                return View();
            }
            catch (Exception ex)
            {
                //throw ex;
                //Redirect to Login page If user is not login                           
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult ChangePassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ;
            }
        }


        public FileStreamResult GetUserManual()
        {
            FileStream usrManual = new FileStream("wwwroot\\fileuploads\\PulseLogicHMR_UserManual.pdf", FileMode.Open);
            return new FileStreamResult(usrManual,"application/pdf");
        }


    }
}
