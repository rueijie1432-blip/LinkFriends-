using Microsoft.Extensions.Configuration;
using System;

namespace MVC_DB_.Models
{
    public class Database
    {
        // 共用資料庫連線字串
        public static string ConnStr { get; private set; } = "";

        // 初始化資料庫連線設定
        public static void Initialize(IConfiguration configuration)
        {
            // 從 appsettings.json 讀取 DefaultConnection
            ConnStr = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("找不到資料庫連線設定");
        }
    }
}