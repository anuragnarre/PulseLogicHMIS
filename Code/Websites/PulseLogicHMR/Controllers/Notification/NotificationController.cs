using PulseLogicHMR.Core.Configuration;
using PulseLogicHMR.DalLayer;
using PulseLogicHMR.Enums;
using PulseLogicHMR.Security;
using PulseLogicHMR.ServerModel.NotificationModels;
using PulseLogicHMR.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PulseLogicHMR.Controllers
{

    public class NotificationController : CommonController
    {
        private readonly NotiFicationDbContext _notificationDbContext;
        private readonly PulseLogicHMR.Core.Utilities.Messaging.IMessagingService _messagingService;

        double cacheExpMinutes;//= 5;//this should come from configuration later on.

        public NotificationController(IOptions<MyConfiguration> _config, PulseLogicHMR.Core.Utilities.Messaging.IMessagingService messagingService) : base(_config)
        {
            cacheExpMinutes = _config.Value.CacheExpirationMinutes;
            _notificationDbContext = new NotiFicationDbContext(connString);
            _messagingService = messagingService;
        }



        //[HttpGet]
        //public string Get(string reqType, int notificationId)
        //{
        //    PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();


        //    NotiFicationDbContext notificationDbContext = new NotiFicationDbContext(connString);
        //    require current login User we are getting from Session
        //    RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");

        //    int EmployeeId = currentUser.EmployeeId;
        //    try
        //    {


        //        {
        //            var lastOneWeekDate = DateTime.Now.Date.AddDays(-7);
        //            //get all NOT-ARCHIVED notifications upto last week into a local variable--LINQ JOIN doesn't work with DBSet and InMemory list at once.
        //            var activeNotifications = notificationDbContext.Notifications
        //                .Where(n => n.RecipientId != null && n.IsArchived == false && n.CreatedOn >= lastOneWeekDate).ToList();

        //            List<NotificationViewModel> userNotifications = (from notf in activeNotifications
        //                                                             where notf.RecipientType == "user" &&
        //                                                             notf.RecipientId == EmployeeId ///display notification against current logged in User
        //                                                             orderby notf.NotificationId descending ///orderby is required because we have to show the latest notification on TOP position
        //                                                             select notf).ToList();



        //            List<RbacRole> currUsrsRoles = HttpContext.Session.Get<List<RbacRole>>("user-roles");
        //            List<NotificationViewModel> userRoleNotifcns = (from notf in activeNotifications
        //                                                            join role in currUsrsRoles
        //                                                             on notf.RecipientId equals role.RoleId
        //                                                            where notf.RecipientType == "rbac-role"
        //                                                            select notf
        //                                                           ).ToList();

        //            List<NotificationViewModel> mergedList = userNotifications.Concat(userRoleNotifcns).OrderByDescending(n => n.CreatedOn).Distinct().ToList();

        //            //if (userNotifications != null)
        //            //{
        //            //}
        //            //else
        //            //{
        //            //    mergedList= 
        //            //}


        //            responseData.Results = mergedList;
        //            responseData.Status = "OK";
        //        }

        //        else

        //        {
        //            var visitDetail = (from notification in notificationDbContext.Notifications
        //                               join visit in notificationDbContext.PatientVisits on notification.NotificationParentId equals visit.PatientVisitId
        //                               where notification.NotificationId == notificationId
        //                               select new
        //                               {
        //                                   PatientId = visit.PatientId,
        //                                   PatientVisitId = visit.PatientVisitId,
        //                                   PerformerId = visit.PerformerId
        //                               }).FirstOrDefault();
        //            responseData.Results = visitDetail;
        //            responseData.Status = "OK";
        //        }
        //        else
        //        {
        //            responseData.Status = "Failed";
        //            responseData.ErrorMessage = "Invalid Request Type";
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //        responseData.Results = null;
        //        responseData.Status = "Failed";
        //        responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
        //    }

        //    return PulseLogicJSONConvert.SerializeObject(responseData, true);

        //}


        [HttpGet]
        [Route("UserNotifications")]
        public IActionResult GetUserNotifications()
        {
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            int EmployeeId = currentUser.EmployeeId;
            Func<object> func = () => GettingUserNotifications(EmployeeId);
            return InvokeHttpGetFunction(func);
        }
       
        [HttpGet]
        [Route("VisitNotificaionDetail")]
        public IActionResult GetVisitNotificaionDetail(int notificationId)
        {
            Func<object> func = () => GettingVisitNotificaionDetail(notificationId);
            return InvokeHttpGetFunction(func);
        }
      




        [HttpPost]
        [Route("SendWhatsAppMessage")]
        public async System.Threading.Tasks.Task<IActionResult> SendWhatsAppMessage([FromBody] dynamic payload)
        {
            try
            {
                string phoneNumber = payload.phoneNumber;
                string message = payload.message;

                if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(message))
                {
                    return BadRequest(new { Status = "Error", ErrorMessage = "Phone number and message are required." });
                }

                bool result = await _messagingService.SendWhatsAppAsync(phoneNumber, message);
                return Ok(new { Status = "OK", Results = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Status = "Error", ErrorMessage = ex.Message });
            }
        }

        [HttpPut]
        [Route("MarkAsRead")]
        public IActionResult MarkAsRead ()
        {
            string ipDataStr = this.ReadPostData();
            RbacUser currentUser = HttpContext.Session.Get<RbacUser>(ENUM_SessionVariables.CurrentUser);
            int EmployeeId = currentUser.EmployeeId;
            Func<object> func = () => UpdateNotificationAsRead(ipDataStr, EmployeeId);
            return InvokeHttpPutFunction(func);
        }
       

        [HttpPut]
        [Route("Archive")]
        public IActionResult Archive()
        {
            string ipDataStr = this.ReadPostData();
            Func<object> func = () => ArchiveNotification(ipDataStr);
            return InvokeHttpPutFunction(func);
        }
       
    




    //    [HttpPost]// POST api/values
    //    public string Post()
    //    {
    //        return null;
    //    }
    //    [HttpPut]
    //    public string Put()
    //    {
    //        PulseLogicHTTPResponse<object> responseData = new PulseLogicHTTPResponse<object>();

    //        try
    //        {

    //            string str = this.ReadPostData();
    //            string reqType = this.ReadQueryStringData("reqType");

    //            NotiFicationDbContext notificationDbContext = new NotiFicationDbContext(connString);
    //            RbacUser currentUser = HttpContext.Session.Get<RbacUser>("currentuser");
    //            int EmployeeId = currentUser.EmployeeId;


    //            //if (reqType == "mark-as-read")
    //            //{
    //            //    List<NotificationViewModel> messageList = PulseLogicJSONConvert.DeserializeObject<List<NotificationViewModel>>(str);
    //            //    if (messageList != null && messageList.Count > 0)
    //            //    {
    //            //        foreach (NotificationViewModel msg in messageList)
    //            //        {
    //            //            msg.IsRead = true; ///update IsRead property 
    //            //            msg.ReadBy = EmployeeId; ///update Readby property by current loggedIn User
    //            //            notificationDbContext.Notifications.Attach(msg);

    //            //            notificationDbContext.Entry(msg).Property(x => x.IsRead).IsModified = true;
    //            //            notificationDbContext.Entry(msg).Property(x => x.ReadBy).IsModified = true;
    //            //        }
    //            //    }

    //            //    notificationDbContext.SaveChanges();

    //            //    responseData.Results = messageList;
    //            //    responseData.Status = "OK";

    //            //}
    //            //else


    //            {
    //                List<NotificationViewModel> messageList = PulseLogicJSONConvert.DeserializeObject<List<NotificationViewModel>>(str);
    //                if (messageList != null && messageList.Count > 0)
    //                {
    //                    foreach (NotificationViewModel msg in messageList)
    //                    {
    //                        msg.IsArchived = true; ///update IsArchived property 
    //                        notificationDbContext.Notifications.Attach(msg);
    //                        notificationDbContext.Entry(msg).Property(x => x.IsArchived).IsModified = true;
    //                    }
    //                }

    //                notificationDbContext.SaveChanges();
    //                responseData.Results = messageList;
    //                responseData.Status = "OK";

    //            }
    //            else
    //            {
    //                responseData.Status = "Failed";
    //                responseData.ErrorMessage = "Invalid Request Type";
    //            }


    //        }
    //        catch (Exception ex)
    //        {
    //            responseData.Status = "Failed";
    //            responseData.ErrorMessage = ex.Message + " exception details:" + ex.ToString();
    //        }
    //        return PulseLogicJSONConvert.SerializeObject(responseData, true);
    //    }

    //}
    private object GettingUserNotifications(int EmployeeId)
    {
        var lastOneWeekDate = DateTime.Now.Date.AddDays(-7);
        var activeNotifications = _notificationDbContext.Notifications
            .Where(n => n.RecipientId != null && n.IsArchived == false && n.CreatedOn >= lastOneWeekDate).ToList();

        List<NotificationViewModel> userNotifications = (from notf in activeNotifications
                                                         where notf.RecipientType == "user" &&
                                                         notf.RecipientId == EmployeeId
                                                         orderby notf.NotificationId descending
                                                         select notf).ToList();



        List<RbacRole> currUsrsRoles = HttpContext.Session.Get<List<RbacRole>>("user-roles");
        List<NotificationViewModel> userRoleNotifcns = (from notf in activeNotifications
                                                        join role in currUsrsRoles
                                                         on notf.RecipientId equals role.RoleId
                                                        where notf.RecipientType == "rbac-role"
                                                        select notf
                                                       ).ToList();

        List<NotificationViewModel> mergedList = userNotifications.Concat(userRoleNotifcns).OrderByDescending(n => n.CreatedOn).Distinct().ToList();
        return mergedList;
    }
    private object GettingVisitNotificaionDetail(int notificationId)
        {

            var visitDetail = (from notification in _notificationDbContext.Notifications
                               join visit in _notificationDbContext.PatientVisits on notification.NotificationParentId equals visit.PatientVisitId
                               where notification.NotificationId == notificationId
                               select new
                               {
                                   PatientId = visit.PatientId,
                                   PatientVisitId = visit.PatientVisitId,
                                   PerformerId = visit.PerformerId
                               }).FirstOrDefault();
            return visitDetail;
        }
    private object UpdateNotificationAsRead(string ipDataStr, int EmployeeId)
        {
            List<NotificationViewModel> messageList = PulseLogicJSONConvert.DeserializeObject<List<NotificationViewModel>>(ipDataStr);
            if (messageList != null && messageList.Count > 0)
            {
                foreach (NotificationViewModel msg in messageList)
                {
                    msg.IsRead = true;
                    msg.ReadBy = EmployeeId;
                    _notificationDbContext.Notifications.Attach(msg);
                    _notificationDbContext.Entry(msg).Property(x => x.IsRead).IsModified = true;
                    _notificationDbContext.Entry(msg).Property(x => x.ReadBy).IsModified = true;
                }
            }

            _notificationDbContext.SaveChanges();

            return messageList;
        }
    private object ArchiveNotification(string ipDataStr)
        {
            List<NotificationViewModel> messageList = PulseLogicJSONConvert.DeserializeObject<List<NotificationViewModel>>(ipDataStr);
            if (messageList != null && messageList.Count > 0)
            {
                foreach (NotificationViewModel msg in messageList)
                {
                    msg.IsArchived = true;
                    _notificationDbContext.Notifications.Attach(msg);
                    _notificationDbContext.Entry(msg).Property(x => x.IsArchived).IsModified = true;
                }
            }

            _notificationDbContext.SaveChanges();
            return messageList;

        }


    }
}

