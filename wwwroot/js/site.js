// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
//for cart count
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

function AddToCart(id) {
    if (id != undefined && id != null) {
        $.ajax({
            url: '@Url.Action("Buy","Cart")',
            type: 'Get',
            data: { Pid: id },
            success: function (data) {
                $("#value").text(data);
                //alert('Product added to cart!');
            },
            error: function () {
                alert('An error occurred while adding the product to cart.');
            }
        });
    }
}

//for search button
function SearchProduct() {
    $.ajax({
        url: "/Home/GetProducts?currentPageIndex=1&Search=" + $('#Searchtext').val(),
        success: function (result) {
            $("#productgrid").html(result);
        }
    });
}