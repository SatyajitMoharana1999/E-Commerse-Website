
$(".pop-show-button").click(function () {
    const id = $(this).attr("pop-target");
    const popup = $("#" + id).find(".pop-up").addClass("p-show");
    $("#" + id).show();

    // Reset the animation
    popup.css("animation", "none");
    setTimeout(() => {
        popup.css("animation", "");
        popup.addClass("p-show");
    }, 10); // Small delay to reapply the animation
})