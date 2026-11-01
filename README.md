## 🌐 線上體驗

[🚀 點此前往 LinkFriends](https://mvcdb20261022215245-grdqdfe2gqdkavh0.westus3-01.azurewebsites.net)

## 📊 專案簡報

[下載 LinkFriends 專案介紹簡報]
[LinkFriends(社群平台).pptx](https://github.com/user-attachments/files/34216730/LinkFriends.pptx)

## 📌 專案介紹

LinkFriends 是一個以社群互動為核心的交友平台，使用者可以建立個人作品集、分享內容，並透過按讚、留言、好友申請及即時聊天與其他使用者交流。

本專案主要目的是整合會員管理、社群互動及即時通訊功能，打造一個具有完整社交功能的平台。

## 🛠️ 使用技術

| 類別 | 技術 |
|---|---|
| 後端 | C#、ASP.NET MVC |
| 前端 | HTML、CSS、JavaScript |
| 前後端互動 | Fetch API、AJAX、JSON |
| 資料庫 | SQL Server |
| 資料庫存取 | ADO.NET |
| 即時通訊 | SignalR |
| 雲端部署 | Microsoft Azure |

## ✨ 系統功能

### 1. 會員管理
- 會員註冊與登入
- 會員資料管理
- Session 使用者狀態管理
- 密碼格式驗證

### 2. 個人作品集
- 新增作品
- 查看作品
- 修改作品
- 刪除作品
- 圖片上傳

### 3. 社群互動
- 作品按讚
- 作品留言
- 好友申請
- 好友狀態管理

### 4. 通知系統
- 按讚通知
- 留言通知
- 未讀通知提示

### 5. 即時聊天
- SignalR 即時通訊
- 好友之間即時傳送訊息
- 使用 Hub 與 Group 管理連線及訊息傳遞

### 6. 管理員功能
- 管理平台內容
- 封鎖違規作品

## 🏗️ 系統架構

本專案採用 ASP.NET MVC 架構，將系統分為不同職責：

- **Models**：負責資料模型及資料庫存取。
- **Views**：負責前端畫面呈現。
- **Controllers**：接收使用者請求，處理業務邏輯並回傳結果。
- **wwwroot**：存放 CSS、JavaScript、圖片等靜態資源。
- **Hubs**：透過 SignalR 處理即時通訊。

### 前後端資料傳遞

使用 Fetch API 發送非同步 HTTP 請求，後端 Controller 處理資料後，以 JSON 格式回傳結果，前端 JavaScript 接收資料並更新畫面，減少整頁重新載入的需求。

### 即時通訊

使用 SignalR 建立瀏覽器與後端 Hub 之間的即時連線，讓使用者能夠即時接收好友傳送的訊息。

## ☁️ 雲端部署

本專案使用 Microsoft Azure 進行雲端部署：

- Azure App Service：託管 ASP.NET MVC 網站。
- Azure SQL Database：儲存會員、作品及社群互動資料。