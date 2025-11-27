/*const { get } = require("jquery");*/

function AddToCart(itemId,unitPrice,quantity) {
    $.ajax({
        type: "GET",
        url: "Cart/AddToCart/" + itemId + "/" + unitPrice + "/" + quantity,
        success: function (response) {
            $("#cartCounter").text(response.count);
        },
        error: function (event) {
            alert("Error in adding item to the cart")
        }
    })
}

$(document).ready(function () {
    $.ajax({
        type: "GET",
        url: "Cart/GetCartCount",
        success: function (response) {
            $("#cartCounter").text(response.count);
        },
        error: function (event) {
            alert("Error in fetching item from cart")
        }
    })
})