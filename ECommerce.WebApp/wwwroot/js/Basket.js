"use strict";

//= require Common.js

var BasketPage = {

    settings: {
        removeButtons: $(".removeButton"),
        purchaseButton: $(".purchaseButton")
    },

    init: function () {

        this.settings.removeButtons.click(function (e) {
            var itemToRemove = document.getElementsByName('ItemToRemove')[0];
            itemToRemove.value = e.target.value;
        });

        this.settings.purchaseButton.click(function (e) {
            // Purchase is implemented on form post
        });
    }
};

(function () {

    BasketPage.init();

})();

