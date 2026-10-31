using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using System.Collections.Generic;

namespace MVC_DB_.Controllers
{
    public class PortfolioLikeController : Controller
    {
        // Create a like if it doesn't exist, otherwise delete the like
        [HttpPost]
        public IActionResult Like(int portfolioID)
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");
            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }
            PortfolioLikeDB portfolioLikeDB = new PortfolioLikeDB();
            PortfolioLike existingLike = portfolioLikeDB.GetPortfolioLike(portfolioID, accountID.Value);
            if (existingLike == null)
            {
                portfolioLikeDB.CreatePortfolioLike(new PortfolioLike
                {
                    portfolioID = portfolioID,
                    accountID = accountID.Value
                });

                // 建立通知
                CreateNotification(portfolioID, accountID.Value);

            }
            else
            {
                portfolioLikeDB.DeletePortfolioLike(existingLike.id);
            }

            int likeCount = portfolioLikeDB.GetPortfolioLikesCount(portfolioID);

            return Json(new
            {
                success = true,
                likeCount = likeCount
            });
        }

        // Create Notification
        // GetPortfolioById的
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
                    notificationType = "like"           
                });
            }
        }


    }
}
