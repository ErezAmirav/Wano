$(function () {
    // previous detection logic

    $("#to-top").on("click", function () {
        $("html, body").animate({
            scrollTop: 0
        }, 200);
    });
});