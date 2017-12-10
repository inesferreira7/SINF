
$('.add-to-cart').on('click', function () {
    var cart = $('.shopping-cart');
    
    var imgtodrag = $(this).parent('.item').find("img").eq(0);
    
    if (imgtodrag && cart[0]) {
        
        var imgclone = imgtodrag.clone()
            .offset({
                top: imgtodrag.offset().top,
                left: imgtodrag.offset().left
            })
            .css({
                'opacity': '0.5',
                'position': 'absolute',
                'height': '150px',
                'width': '150px',
                'z-index': '100'
            })
            
            .appendTo($('body'))
            .animate({
                'top': cart.offset().top + 10,
                'left': cart.offset().left + 10,
                'width': 75,
                'height': 75
            }, 1000, 'easeInOutExpo');
            
      

        imgclone.animate({
            'width': 0,
            'height': 0
        }, function () {
            $(this).detach()
        });
    }

    var data = $(this).attr("product_id").toString();
    var dataToPost = {
        CodArtigo: data
    }

    $.post("/ShoppingCart/AddDefaultToCart", dataToPost)
            .done(function (r) {

            })
            .fail(function (r) {
                console.log("Failed to add the product to the shopping cart");
            })
});

