using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MVC_DB_.Models;
using System;
using MVC_DB_.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace MVC_DB_.Controllers
{
    public class FriendMessageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // CREATE
        [HttpPost]
        public IActionResult Create(int friendID, string message)
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
            FriendMessage friendMessage = new FriendMessage
            {
                friendID = friendID,
                senderAccountID = accountID.Value,
                message = message,
                createdAt = DateTime.Now
            };
            FriendMessageDB friendMessageDB = new FriendMessageDB();
            friendMessageDB.CreateFriendMessage(friendMessage);
            return Json(new
            {
                success = true
            });
        }

        // READ
        public IActionResult GetFriendMessages(int friendID)
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
            FriendMessageDB friendMessageDB = new FriendMessageDB();
            var messages = friendMessageDB.GetFriendMessages(friendID);
            return Json(new
            {
                success = true,
                messages = messages
            });
        }


    }
}
