using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MVC_DB_.Controllers
{
    public class PortfolioCommentController : Controller
    {

        // Create a new comment
        [HttpPost]
        public IActionResult Create(int portfolioID, string comment)
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
            if (string.IsNullOrWhiteSpace(comment))
            {
                return Json(new
                {
                    success = false,
                    message = "留言不能為空"
                });
            }

            PortfolioCommentDB portfolioCommentDB = new PortfolioCommentDB();
            int commentID = portfolioCommentDB.CreatePortfolioComment(new PortfolioComment
            {
                comment = comment,
                accountID = accountID.Value,
                portfolioID = portfolioID,
            });
            // ID > UserName
            return Json(new
            {
                success = true,
                id = commentID,
                comment = comment,
                accountID = accountID.Value,
                portfolioID = portfolioID,
                userName = HttpContext.Session.GetString("userName")
            });

            // 留言後建立通知
            CreateNotification(portfolioID, accountID.Value);

            return Json(new
            {
                success = true,
                id = commentID,
                comment = comment,
                accountID = accountID.Value
            });
        }

        // Delete a comment
        [HttpPost]
        public IActionResult Delete(int id, int portfolioID)
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
            PortfolioCommentDB portfolioCommentDB = new PortfolioCommentDB();
            PortfolioComment comment = portfolioCommentDB.GetPortfolioCommentById(id);
            if (comment == null || comment.accountID != accountID.Value)
            {
                return NotFound();
            }
            portfolioCommentDB.DeletePortfolioComment(id);

            return Json(new
            {
                success = true,
                id = id
            });
        }

        // Create Notification for comment
        private void CreateNotification(int portfolioID, int accountID)
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            Portfolio portfolio = portfolioDB.GetPortfolioById(portfolioID);
            if (portfolio != null && portfolio.accountID != accountID)
            {
                NotificationDB notificationDB = new NotificationDB();
                notificationDB.CreateNotification(new Notification
                {
                    accountID = portfolio.accountID,
                    fromAccountID = accountID,
                    portfolioID = portfolioID,
                    notificationType = "comment"
                });
            }
        }

    }
}
