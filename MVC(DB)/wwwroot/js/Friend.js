// READ ALL USERS

fetch("/Friend/GetAllUsers")
    .then(response => response.json())

    .then(data => {
        const userList = document.getElementById("userList");

        if (!data.success) {
            userList.innerHTML = `<p>${data.message}</p>`;
            return;
        }

        if (data.allUsers.length === 0) {
            userList.innerHTML =
                "<p>目前沒有可以添加的好友。</p>";
            return;
        }

        data.allUsers.forEach(user => {

            userList.innerHTML += `
                <div style="border:1px solid #ccc; padding:15px; margin-bottom:10px;">

                    <p>
                        <strong>帳號：</strong>
                        ${user.userName}
                    </p>

                    <p>
                        <strong>姓名：</strong>
                        ${user.name}
                    </p>

                    <button onclick="addFriend(${user.id})">
                        加好友
                    </button>

                </div>
            `;

        });

    });

// CREATE
function addFriend(friendID) {

    fetch("/Friend/Create", {
        method: "POST",

        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },

        body: `friendID=${friendID}`
    })
        .then(response => response.json())
        .then(data => {

            if (data.success) {

                alert("好友邀請已送出");

                location.reload();

            }
            else {

                alert(data.message);

            }

        });

}


// UPDATE

function updateFriend(friendID, status) {



    fetch("/Friend/Update", {
        method: "POST",

        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },

        body: `friendID=${friendID}&status=${status}`
    })
        .then(response => response.json())
        .then(data => {

            if (data.success) {

                if (status == 1) {
                    alert("已接受好友邀請");
                }
                else {
                    alert("已拒絕好友邀請");
                }

                location.reload();

            }
            else {

                alert(data.message);

            }

        });

}

// DELETE
function deleteFriend(friendID) {

    if (!confirm("確定要刪除好友嗎？")) {
        return;
    }

    fetch("/Friend/Delete", {
        method: "POST",

        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },

        body: `friendID=${friendID}`
    })
        .then(response => response.json())
        .then(data => {

            if (data.success) {

                alert("好友已刪除");

                location.reload();

            }
            else {

                alert(data.message);

            }

        });

}