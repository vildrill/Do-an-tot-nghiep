var CartClass = {
    Get: function () {
        var sIDSP = localStorage.getItem("cart") || "[]";
        return JSON.parse(sIDSP);
    },
    Set: function (arr) {
        var jsonIDSP = JSON.stringify(arr);
        localStorage.setItem("cart", jsonIDSP);
    },

    UpdateQuantity: function (productId, newQuantity) {
        var cart = this.Get();
        var updatedCart = cart.map(function (item) {
            if (item.productId === productId) {
                item.quantity = newQuantity;
            }
            return item;
        });
        this.Set(updatedCart);
    },

    addToCart: function (productId, colorValue, sizeValue, quantityValue, priceValue) {
        console.log('Thêm giỏ');
        console.log(productId, colorValue, sizeValue, quantityValue, priceValue);

        var cart = this.Get();

        var existingItem = cart.find(function (item) {
            return item.productId === productId && item.color === colorValue && item.size === sizeValue;
        });

        if (existingItem) {
            // Chuyển đổi existingItem.quantity thành số nếu có thể, nếu không thì gán giá trị 0
            var existingQuantity = isNaN(parseInt(existingItem.quantity, 10)) ? 0 : parseInt(existingItem.quantity, 10);
            // Chuyển đổi quantityValue thành số nếu có thể, nếu không thì gán giá trị 0
            var quantityToAdd = isNaN(parseInt(quantityValue, 10)) ? 0 : parseInt(quantityValue, 10);
            // Thực hiện phép cộng
            existingItem.quantity = existingQuantity + quantityToAdd;
            $('#errorMessage').text('Products already in the cart!');
            $('#errorAlert').fadeIn('slow').delay(2000).fadeOut('slow');
        }
         else {
            var cartItem = {
                productId: productId,
                color: colorValue,
                size: sizeValue,
                quantity: quantityValue,
                price: priceValue
            };
            cart.push(cartItem);
            $('#successMessage').text('The product has been added to cart');
            $('#successAlert').fadeIn('slow').delay(2000).fadeOut('slow');
        }
       
        // Cập nhật giỏ hàng mới vào localStorage
        this.Set(cart);
    }
};
