using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulseLogicHMR.Controllers
{
    public class SchedulingViewController : Controller
    {
        public IActionResult SchedulingMain()
        {
            try
            {
                return View("~/Views/SchedulingView/SchedulingMain.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult ManageMain()
        {
            try
            {
                return View("~/Views/SchedulingView/Manage/ManageMain.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult ManageSchedules()
        {
            try
            {
                return View("~/Views/SchedulingView/Manage/ManageSchedules.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult ManageWorkingHours()
        {
            try
            {
                return View("~/Views/SchedulingView/Manage/ManageWorkingHours.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult SettingMain()
        {
            try
            {
                return View("~/Views/SchedulingView/Setting/SettingMain.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
        public IActionResult ShiftsManage()
        {
            try
            {
                return View("~/Views/SchedulingView/Setting/ShiftsMaster.cshtml");
            }
            catch (Exception ex)
            {
                throw ;
            }
        }
    }
}
