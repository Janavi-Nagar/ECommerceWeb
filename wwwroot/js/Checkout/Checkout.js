$(document).ready(function () {
    //$('#AddressBody').load('/Checkout/CartCoupon');
    $(".discountamt").hide();
    $.ajax({
            url: "/Checkout/GetCartProduct",
            type: "post",            
            success: function (data) {
                $("#CartProducts").html(data);
            }
    });    
});

function SetCartjsondata() {
    var DataList = [];
    $('.ecartitems').each(function (index) {
        DataList.push({
            ProductId: $(this).find('#Cart_ProductId').text(),
            UnitPrice: $(this).find('#Cart_UintPrice').text(),
            Quantity: $(this).find('#Cart_Quantity').text(),
            Discount: $(this).find('#Cart_Discount').text()
        })
        $("#CartDetails").val(JSON.stringify(DataList));
    });
}
$("#BtnSave").click(function (e) {
    var val = $("#CodeApply").val();
    var valno = val.length;
    if (valno > 3 && valno < 6) {
        $.ajax({
            url: "/Checkout/ValidateCouponCode",
            type: "post",
            data: { code: val },
            success: function (data) {
                $('.discountpercls').each(function (index) {
                    $(this).text('Discount 0%');
                });
                $('.amountpercls').each(function (index) {
                    $(this).text('0.00');
                });
                $(".Promocode").text("");
                $(".discountamt").text("0.00%");
                $(".finalamt").text($("#productamt").text());
                $("#discountamount").val(0);
                $("#grossamount").val(0);
                $("#discount").val(0);
                $("#Net").val(0);
                $("#Cart_Discount").text("0");

                if (data != null) {
                    var discountfinalamt = 0;
                    if (data.cartitem != null) {
                        for (var i = 0; i < data.cartitem.length; i++) {
                            var discountamt = ((parseFloat(data.cartitem[i].products.price) * parseFloat(data.coupon.discount)) / 100);
                            discountfinalamt = discountfinalamt + discountamt;
                            $("#discountper_" + data.cartitem[i].products.productId).text(data.coupon.discount.toFixed(2) + "%");
                            $("#amount_" + data.cartitem[i].products.productId).text("-" + discountamt.toFixed(2));
                        }
                    }
                    $(".Promocode").text(data.coupon.couponCode)
                    var productamt = $("#productamt").text().replace(/,/g, "");;
                    $(".discountamt").text("-" + discountfinalamt.toFixed(2));
                    $("#discountamount").val(discountfinalamt.toFixed(2));
                    $(".finalamt").text((parseFloat(productamt) - discountfinalamt).toFixed(2));
                    $("#grossamount").val((parseFloat(productamt) - discountfinalamt).toFixed(2));
                    $("#discount").val(parseFloat(data.coupon.discount).toFixed(2));
                    $("#Net").val(parseFloat(productamt).toFixed(2));
                    $("#Cart_Discount").text(data.coupon.discount.toFixed(2));
                } else {
                    alert("Coupon code is not validate");                
                }
            }
        });
    } else {
        alert("please enter valid coupon code min 3 and max 5.");
    }
    SetCartjsondata();
    
});