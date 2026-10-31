document.querySelectorAll("form[id^='likeForm-']").forEach(function (form) {

    form.addEventListener("submit", function (event) {

        event.preventDefault();

        const formData = new FormData(form);

        // AJAX
        fetch(form.action, {
            method: "POST",
            body: formData
        })
            .then(response => response.json())
            .then(data => {

                if (!data.success) {
                    alert(data.message);
                    return;
                }

                const portfolioID = formData.get("portfolioID");
                const likeCount = document.getElementById("like-count-" + portfolioID);

                likeCount.textContent = data.likeCount;
                
            });
    });
});