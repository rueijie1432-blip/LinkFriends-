namespace MVC_DB_.Models
{
    public class PortfolioComment
    {
        public int id { get; set; }
        public string comment { get; set; }
        public int accountID { get; set; }
        public int portfolioID { get; set; }
        public bool isBlocked { get; set; }
    }
}
