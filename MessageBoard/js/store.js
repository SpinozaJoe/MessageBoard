// store.js
(function ()
{
    var storeModule = angular.module('store', []);
    var gems = [{
        name: 'Dodechahedron',
        price: 2,
        description: 'awesomeness',
        specification: '4 inches and some stuff',
        canPurchase: true,
        soldOut: false,
        reviews: [{
            stars: 2,
            body: 'No bad actually',
            author: "spinman"
        }, {
            stars: 5,
            body: 'Aces',
            author: "plearthom"
        }]
    }, {
        name: 'Pentagonal Gem',
        price: 5.95,
        description: 'Far cooler than the sun',
        specification: '2 inches but most cool',
        canPurchase: false,
        soldOut: false,
        reviews: []
    }];

    storeModule.controller('StoreController', function ()
    {
        this.products = gems;
    });

    storeModule.controller('PanelController', function ()
    {
        this.tab = 1;

        this.selectTab = function (tabId)
        {
            this.tab = tabId;
        };

        this.tabIsSelected = function (tabId)
        {
            return this.tab === tabId;
        };

    });

    storeModule.controller('ReviewController', function ()
    {
        this.newReview = {};

        this.addReview = function (product)
        {
            product.reviews.push(this.newReview);
            this.newReview = {};
        };

    });

    storeModule.directive('productTitle', function ()
    {
        return {
            // E stands for Element (A is for Attribute)
            restrict: 'E',
            templateUrl: '../templates/product-title.html'
        };
    });

})();