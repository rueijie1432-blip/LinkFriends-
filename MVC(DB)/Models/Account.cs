using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_DB_.Models
{
    public class account
    {
        public int id { get; set; }
        [Required(ErrorMessage = "帳號不能為空")]
        public string userName { get; set; }
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度必須為 8～20 個字")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d).+$",ErrorMessage = "密碼必須包含英文和數字")]
        [Required(ErrorMessage = "密碼不能為空")]
        public string passwd { get; set; }
        [Required(ErrorMessage = "姓名不能為空")]
        public string name { get; set; }
        public string avatar { get; set; }
        public bool isAdmin { get; set; }
    }

}
