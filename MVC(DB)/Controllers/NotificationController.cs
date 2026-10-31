using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using System.Collections.Generic;

namespace MVC_DB_.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");

            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            NotificationDB notificationDB = new NotificationDB();
            List<Notification> notifications =
                 notificationDB.GetNotifications(accountID.Value);

            return View(notifications);
        }

        // update notification all to read
        [HttpPost]
        public IActionResult MarkAllAsRead()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return Json(new
                {
                    success = false,
                    message = "請先登入"
                });
            }
            NotificationDB notificationDB = new NotificationDB();
            notificationDB.MarkAllAsRead(accountID.Value);
            return Json(new
            {
                success = true
            });
        }

    }
}
