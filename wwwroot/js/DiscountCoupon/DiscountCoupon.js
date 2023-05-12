//update coupon
function GetDetails(id) {
    $.ajax({
        url: '/DiscountCoupon/GetCouponInfo?id=' + id,
        type: "GET",
        success: function (data) {
            $("#CouponBody").html(data);

            //$('#modal-Update').modal('show');
        },
        error: function () { alert('Error'); }
    });
    return false;
}
//delete coupon

function validateData(id) {
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this imaginary file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: '/DiscountCoupon/DeleteCoupon?id=' + id,
                    type: "GET",
                    success: function (data) {
                        window.location.reload();
                    },
                    error: function () { alert('Error'); }
                });

            } else {
                swal("Cancelled", "Your imaginary file is safe :)", "error");
            }

        });
}