function AddToCart(ItemId, Name, UnitPrice, Quantity) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Cart/AddToCart/' + ItemId + "/" + UnitPrice + "/" + Quantity,
        success: function (d) {
            if (response.status === 'success') {
                var counter = response.count;
                $("#cartCounter").text(counter);

                var message = '<strong>' + Name + '</strong> Added to <a href="/cart">Cart</a> Successfully!'
                $('#toastCart > .toast-body').html(message)
                $('#toastCart').toast('show');
                setTimeout(function () {
                    $('#toastCart').toast('hide');
                }, 4000);
            }
        },
        error: function (result) {
        }
    });
}
function deleteItem(id) {
    if (id > 0) {
        $.ajax({
            type: "GET",
            url: '/Cart/DeleteItem/' + id,
            success: function (data) {
                if (data > 0) {
                    location.reload();
                }
            }
        });
    }
}
function updateQuantity(id, currentQuantity, quantity) {
    if ((currentQuantity >= 1 && quantity == 1) || (currentQuantity > 1 && quantity == -1)) {
        $.ajax({
            url: '/Cart/UpdateQuantity/' + id + "/" + quantity,
            type: 'GET',
            success: function (response) {
                if (response > 0) {
                    location.reload();
                }
            }
        });
    }
}

$(document).ready(function () {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: '/Cart/GetCartCount',
        success: function (data) {
            $("#cartCounter").text(data);
        },
        error: function (result) {
        },
    });
});