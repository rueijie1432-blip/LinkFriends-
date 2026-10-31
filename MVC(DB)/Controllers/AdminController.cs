using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;

namespace MVC_DB_.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");

            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            AccountDB accountDB = new AccountDB();

            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }

            // 取得管理員頁面的資料統計數量(count)
            var accounts = accountDB.GetAccounts();
            PortfolioDB portfolioDB = new PortfolioDB();
            var portfolios = portfolioDB.GetAllPortfolios();
            PortfolioCommentDB commentDB = new PortfolioCommentDB();
            var comments = commentDB.GetAllPortfolioComments();
            ViewBag.AccountCount = accounts.Count;
            ViewBag.PortfolioCount = portfolios.Count;
            ViewBag.CommentCount = comments.Count;

            return View();
        }

        // READ all accounts(GetAccounts)
        public IActionResult ReadAccounts()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");

            if (accountID == null) {

                return RedirectToAction("Login", "Account");
            }

            AccountDB accountDB = new AccountDB();
            
            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }

            var accounts = accountDB.GetAccounts();

            return View(accounts);
        }

        // READ all portfolios(GetAllPortfolios)
        public IActionResult ReadPortfolios()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            AccountDB accountDB = new AccountDB();
            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }

            PortfolioDB portfolioDB = new PortfolioDB();

            var portfolios = portfolioDB.GetAllPortfolios();

            return View(portfolios);
        }

        // READ all portfoliocomments(GetAllPortfolioComments)
        public IActionResult ReadPortfolioComments()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            AccountDB accountDB = new AccountDB();
            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }
            PortfolioCommentDB commentDB = new PortfolioCommentDB();
            var comments = commentDB.GetAllPortfolioComments();
            return View(comments);
        }

        // SetPortfolioBlock
        public IActionResult SetPortfolioBlock(int id, bool isBlocked)
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            AccountDB accountDB = new AccountDB();
            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }
            PortfolioDB portfolioDB = new PortfolioDB();
            portfolioDB.SetPortfolioBlock(id, isBlocked);
            return RedirectToAction("ReadPortfolios");
        }

        // SetPortfolioCommentBlock
        public IActionResult SetPortfolioCommentBlock(int id, bool isBlocked)
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            AccountDB accountDB = new AccountDB();
            if (!accountDB.IsAdmin(accountID.Value))
            {
                return RedirectToAction("Index", "Home");
            }
            PortfolioCommentDB commentDB = new PortfolioCommentDB();
            commentDB.SetPortfolioCommentBlock(id, isBlocked);

            return RedirectToAction("ReadPortfolioComments");
        }



    }
}
