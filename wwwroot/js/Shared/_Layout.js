function SearchProduct() {
    $.ajax({
        url: "/Home/GetProduct?currentPageIndex=1&searchtext=" + $("#Searchtext").val(),
        success: function (result) {
            $("#productgrid").html(result);
        }
    });
}

$(document).ready(function () {
    $.ajax({
        url: '/Home/NoOfCartProduct',
        type: "GET",
        success: function (data) {
            $("#value").text(data);
        },
        error: function (error) {
            console.log(`Error ${error}`);
        }
    });
});