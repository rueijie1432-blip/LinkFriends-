using System.ComponentModel.DataAnnotations;

namespace MVC_DB_.Models
{
    public class Portfolio
    {
        public int id { get; set; }
        [Required(ErrorMessage = "標題不能為空")]
        public string title { get; set; }
        [Required(ErrorMessage = "介紹不能為空")]
        public string description { get; set; }
        public string image { get; set; }
        public int accountID { get; set; }
        public bool isBlocked { get; set; }

    }
}
