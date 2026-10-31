using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MVC_DB_.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Create");
        }

        // create a new account
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(account account)
        {
            if (!ModelState.IsValid)
            {
                return View(account);
            }

            AccountDB accountDB = new AccountDB();
            accountDB.CreateAccount(account);
            return RedirectToAction("Index");
        }

        // Read account by id
        public IActionResult Details()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            AccountDB accountDB = new AccountDB();
            account account = accountDB.GetAccountById(accountID.Value);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // Update account
        public IActionResult Edit()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            AccountDB accountDB = new AccountDB();
            account account = accountDB.GetAccountById(accountID.Value);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        [HttpPost]
        public IActionResult Edit(account user, IFormFile avatarFile)
        {
            ModelState.Remove("passwd");
            ModelState.Remove("userName");

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            AccountDB accountDB = new AccountDB();

            account oldAccount = accountDB.GetAccountById(user.id);

            if (avatarFile != null)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/avatar"
                );

                string fileName = Guid.NewGuid().ToString()
                                 + Path.GetExtension(avatarFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    avatarFile.CopyTo(stream);
                }

                if (!string.IsNullOrEmpty(oldAccount.avatar)
                    && oldAccount.avatar != "default-avatar.png")
                {
                    string oldFilePath = Path.Combine(
                        folder,
                        oldAccount.avatar
                    );

                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                user.avatar = fileName;
            }
            else
            {
                user.avatar = oldAccount.avatar;
            }

            accountDB.UpdateAccount(user);

            return RedirectToAction("Details");
        }


        // Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string passwd)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passwd))
            {
                ViewBag.Error = "帳號和密碼不能為空";
                return View();
            }

            AccountDB accountDB = new AccountDB();
            account user = accountDB.Login(userName, passwd);
            if (user != null)
            {
                HttpContext.Session.SetString("userName", user.userName);
                HttpContext.Session.SetInt32("accountID", user.id);

                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "帳號或密碼錯誤";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

    }
}
