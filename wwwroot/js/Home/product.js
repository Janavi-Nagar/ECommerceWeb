//add to cart
function AddToCart(id) {
    if (id != undefined && id != null) {
        $.ajax({
            url: '/cart?',
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