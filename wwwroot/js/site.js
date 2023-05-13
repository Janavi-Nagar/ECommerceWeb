// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//for cart count


//for search button
function SearchProduct() {
    $.ajax({
        url: "/Home/GetProducts?currentPageIndex=1&Search=" + $('#Searchtext').val(),
        success: function (result) {
            $("#productgrid").html(result);
        }
    });
}