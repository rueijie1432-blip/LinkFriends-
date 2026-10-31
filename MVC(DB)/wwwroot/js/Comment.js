document.addEventListener("DOMContentLoaded", function () {

    // Create 留言
    const commentForms = document.querySelectorAll(
        "form[id^='commentForm-']"
    );

    commentForms.forEach(form => {

        form.addEventListener("submit", function (event) {

            // 阻止表單原本的跳頁
            event.preventDefault();

            const formData = new FormData(form);

            fetch(form.action, {
                method: "POST",
                body: formData
            })
                .then(response => response.json())
                .then(data => {

                    if (data.success) {

                        const portfolioID =
                            formData.get("portfolioID");

                        const commentsContainer =
                            document.getElementById(
                                `comments-${portfolioID}`
                            );

                        const newComment =
                            document.createElement("div");

                        newComment.classList.add("comment-item");

                        newComment.innerHTML = `
                        <small>使用者：${data.userName}</small>

                            <button type="button"
                                    class="delete-comment"
                                    data-id="${data.id}"
                                    data-portfolio-id="${portfolioID}">
                                刪除
                            </button>
                        <p>${data.comment}</p>

                        <hr />
                    `;
                        commentsContainer.prepend(newComment);
                        // 清空輸入框
                        form.querySelector(
                            "input[name='comment']"
                        ).value = "";
                    }
                    else {
                        alert(data.message);
                    }

                });

        });

    });

    // Delete 刪除留言
    document.addEventListener("click", function (event) {

        if (!event.target.classList.contains("delete-comment")) {
            return;
        }

        const button = event.target;

        const id = button.dataset.id;
        const portfolioID = button.dataset.portfolioId;

        const formData = new FormData();

        formData.append("id", id);
        formData.append("portfolioID", portfolioID);

        fetch("/PortfolioComment/Delete", {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {

                if (data.success) {

                    // 把這則留言從畫面移除
                    button.closest(".comment-item").remove();

                }
                else {
                    alert(data.message);
                }

            });

    });

});