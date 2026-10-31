using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_DB_.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVC_DB_.Controllers
{
    public class PortfolioController : Controller
    {

        public IActionResult Index()
        {
            int? accountID = HttpContext.Session.GetInt32("accountID");

            if (accountID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            PortfolioDB portfolioDB = new PortfolioDB();

            List<Portfolio> portfolios = portfolioDB.GetPortfolios(accountID.Value);

            return View(portfolios);
        }

        public IActionResult Create()
        {
            return View();
        }

        // Create a new portfolio
        [HttpPost]
        public IActionResult Create(Portfolio portfolio, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
            {
                return View(portfolio);
            }

            if (imageFile != null)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images/portfolio"
                );

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                portfolio.image = "/images/portfolio/" + fileName;
            }

            int? accountID = HttpContext.Session.GetInt32("accountID");
            portfolio.accountID = accountID.Value;

            PortfolioDB portfolioDB = new PortfolioDB();
            portfolioDB.CreatePortfolio(portfolio);
            return RedirectToAction("Index");
        }

        // Read a portfolio by id
        public IActionResult Details(int id)
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            Portfolio portfolio = portfolioDB.GetPortfolioById(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        // Update a portfolio
        public IActionResult Edit(int id)
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            Portfolio portfolio = portfolioDB.GetPortfolioById(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        [HttpPost]
        public IActionResult Edit(Portfolio portfolio)
        {
            PortfolioDB portfolioDB = new PortfolioDB();

            portfolioDB.UpdatePortfolio(portfolio);

            return RedirectToAction("Index");
        }

        // Delete a portfolio
        public IActionResult Delete(int id)
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            Portfolio portfolio = portfolioDB.GetPortfolioById(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            PortfolioDB portfolioDB = new PortfolioDB();

            portfolioDB.DeletePortfolio(id);

            return RedirectToAction("Index");
        }

        // PlatformIndex + LikesCount + Comments + ID → Username
        public IActionResult PlatformIndex()
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            PortfolioLikeDB portfolioLikeDB = new PortfolioLikeDB();
            PortfolioCommentDB portfolioCommentDB = new PortfolioCommentDB();
            // ID → Username
            AccountDB accountDB = new AccountDB();

            List<Portfolio> portfolios = portfolioDB.GetAllPortfolios();
            // ID → Username
            List<account> accounts = accountDB.GetAccounts();

            ViewBag.LikesCount = new Dictionary<int, int>();
            ViewBag.Comments = new Dictionary<int, List<PortfolioComment>>();

            // ID → Username
            ViewBag.UserNames = accounts.ToDictionary(
                a => a.id,
                a => a.userName
            );

            foreach (var portfolio in portfolios)
            {
                // Likes
                int count = portfolioLikeDB.GetPortfolioLikesCount(portfolio.id);
                ViewBag.LikesCount[portfolio.id] = count;

                // Comments
                List<PortfolioComment> comments =
                    portfolioCommentDB.GetPortfolioComments(portfolio.id);
                ViewBag.Comments[portfolio.id] = comments;
            }

            return View(portfolios);
        }

        // PlatformDetails
        public IActionResult PlatformDetails(int id)
        {
            PortfolioDB portfolioDB = new PortfolioDB();
            Portfolio portfolio = portfolioDB.GetPortfolioById(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

    }
}