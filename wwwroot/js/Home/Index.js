$(document).ready(function () {
    PagerClick(1)
});

function PagerClick(index) {
    $.ajax({
        url: "/Home/GetProduct?currentPageIndex=" + index + "&searchtext=" + $("#Searchtext").val(),
        success: function (result) {
            $("#productgrid").html(result);
        }
    });
}