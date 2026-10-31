const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", function (friendID, senderAccountID, message) {

    // 自己的訊息不要再次顯示
    if (senderAccountID === currentAccountID) {
        return;
    }

    const messageList = document.getElementById("messageList");

    messageList.innerHTML += `
        <div style="text-align:left; margin-bottom:10px;">

            <div style="font-size:12px; color:#666;">
                ${senderAccountID}
            </div>

            <span style="
                display:inline-block;
                background:#eee;
                padding:8px 12px;
                border-radius:15px;
            ">
                ${message}
            </span>

        </div>
    `;

    messageList.scrollTop = messageList.scrollHeight;
});

connection.start()
    .then(() => {
        console.log("SignalR 已連線");
    })  
    .catch(err => {
        console.error("SignalR 連線失敗:", err);
    });

let currentFriendID = null;

// CREATE 聊天視窗
async function openChat(friendID) {

    currentFriendID = friendID;

    await connection.invoke("JoinChat", friendID);

    document.getElementById("friendMessageBox").style.display = "block";

    // 歷史訊息
    loadMessages();
}

// READ 聊天內容
// READ 聊天內容
async function loadMessages() {

    const response = await fetch(
        `/FriendMessage/GetFriendMessages?friendID=${currentFriendID}`
    );

    const data = await response.json();

    const messageList = document.getElementById("messageList");

    messageList.innerHTML = "";

    data.messages.forEach(message => {

        if (message.senderAccountID === currentAccountID) {

            // 自己 → 右邊
            messageList.innerHTML += `
                <div style="text-align:right; margin-bottom:10px;">

                    <div style="font-size:12px; color:#666;">
                        ${message.senderAccountID}
                    </div>

                    <span style="
                        display:inline-block;
                        background:#DCF8C6;
                        padding:8px 12px;
                        border-radius:15px;
                    ">
                        ${message.message}
                    </span>

                    <div style="font-size:11px; color:#999;">
                        ${new Date(message.createdAt).toLocaleString()}
                    </div>

                </div>
            `;

        } else {

            // 對方 → 左邊
            messageList.innerHTML += `
                <div style="text-align:left; margin-bottom:10px;">

                    <div style="font-size:12px; color:#666;">
                        ${message.senderAccountID}
                    </div>

                    <span style="
                        display:inline-block;
                        background:#eee;
                        padding:8px 12px;
                        border-radius:15px;
                    ">
                        ${message.message}
                    </span>

                    <div style="font-size:11px; color:#999;">
                        ${new Date(message.createdAt).toLocaleString()}
                    </div>

                </div>
            `;

        }

    });

    messageList.scrollTop = messageList.scrollHeight;
}

// UPDATE 送出訊息
async function sendMessage() {

    const message = document.getElementById("messageInput").value;


    if (message.trim() === "") {
        return;
    }

    const response = await fetch("/FriendMessage/Create", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: `friendID=${currentFriendID}&message=${encodeURIComponent(message)}`
    });

    const data = await response.json();

    if (data.success) {

        // 傳給 SignalR ChatHub
        await connection.invoke(
            "SendMessage",
            currentFriendID,
            currentAccountID,
            message
        );

        document.getElementById("messageInput").value = "";
        loadMessages();
    }
}

// DELETE 關閉視窗
function closeChat() {
    document.getElementById("friendMessageBox").style.display = "none";
}

// 按ENTER 呼叫 sendMessage()
document.getElementById("messageInput").addEventListener("keydown", function (event) {  

    if (event.key === "Enter") {
        sendMessage();
    }

});