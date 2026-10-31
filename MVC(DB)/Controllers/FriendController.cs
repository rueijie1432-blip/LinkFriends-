using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using System.Collections.Generic;

namespace MVC_DB_.Controllers
{
    public class FriendController : Controller
    {
        public IActionResult Index()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            FriendDB friendDB = new FriendDB();
            List<Friend> friends = friendDB.GetFriends(accountID.Value);

            return View(friends);

        }

        // Create a new friend request
        [HttpPost]
        public IActionResult Create(int friendID)
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

            Friend friend = new Friend
            {
                senderAccountID = accountID.Value,
                receiverAccountID = friendID,
                status = 0
            };

            FriendDB friendDB = new FriendDB();
            friendDB.CreateFriend(friend);

            return Json(new
            {
                success = true
            });
        }

        // READ WHO SENT FRIEND REQUESTS
        public IActionResult GetFriendRequests()
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
            FriendDB friendDB = new FriendDB();
            List<Friend> friendRequests = friendDB.GetFriendRequests(accountID.Value);
            return Json(new
            {
                success = true,
                friendRequests = friendRequests
            });
        } 

        // UPDATE
        [HttpPost]
        public IActionResult Update(int friendID, int status)
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

            FriendDB friendDB = new FriendDB();

            // status = 2 改成 DELETE
            if (status == 2)
            {
                friendDB.DeleteFriend(friendID, accountID.Value);

                return Json(new
                {
                    success = true
                });
            }

            friendDB.UpdateFriendStatus(friendID, status, accountID.Value);

            return Json(new
            {
                success = true
            });
        }

        // DELETE
        [HttpPost]
        public IActionResult Delete(int friendID)
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
            FriendDB friendDB = new FriendDB();
            friendDB.DeleteFriend(friendID, accountID.Value);
            return Json(new
            {
                success = true
            });
        }

        // READ ALL USERS
        public IActionResult GetAllUsers()
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
            FriendDB friendDB = new FriendDB();
            List<account> allUsers = friendDB.GetAllUsersExceptFriendsAndSelf(accountID.Value);
            return Json(new
            {
                success = true,
                allUsers = allUsers
            });
        }


    }
}
