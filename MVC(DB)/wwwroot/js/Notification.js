document.addEventListener("DOMContentLoaded", function () {

    const notification = document.querySelector(".notification");

    if (notification) {

        notification.addEventListener("mouseleave", function () {

            fetch("/Notification/MarkAllAsRead", {
                method: "POST"
            })
                .then(response => response.json())
                .then(data => {

                    if (data.success) {

                        // 移除鈴鐺旁邊的未讀數量
                        const notificationCount =
                            document.querySelector(".notification-count");

                        if (notificationCount) {
                            notificationCount.remove();
                        }

                        // 移除每一筆通知旁邊的紅點
                        const notificationDots =
                            document.querySelectorAll(".notification-dot");

                        notificationDots.forEach(function (dot) {
                            dot.remove();
                        });

                    }

                });

        });

    }

});