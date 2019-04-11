'use strict';
app.controller("mapController", ['$scope', 'dataService', '$q', '$uibModal', function ($scope, dataService, $q, $uibModal) {
    $scope.randomMarkers = [];
    var pins = [];
    var i = 0;


    var promotionsList = [];

    var promotionsquery = dataService.getPromotions().query().$promise.then(function (promotions) {

        // Store all promotions 
        for (var i = 0; i < promotions.length; i++) {
            promotions[i].PromotionImageURL = encodeURIComponent(promotions[i].PromotionImageURL)
        }
        promotionsList = promotions;
        var promisesList = [];

        angular.forEach(promotions, function (promo) {
            var vendorId = promo.VendorId;

            promisesList.push(dataService.getVendors().get({
                vendor: vendorId
            }).$promise.then(function (data) {
                pins.push({
                    id: promo.vendorId,
                    title: promo.Title,
                    pos: [data.Location.Lat, data.Location.Lng],
                    promotionDetails: promo
                });
            }, function (error) {
                console.log("vendor not found");
            }));
        });

        $q.all(promisesList).then(function () {
            $scope.promotions = promotions;
            $scope.randomMarkers = pins;
        });

    }, function (error) {
        console.log("Error: Could not load promotions");
    });

    $scope.showPromotionData = function () {

        var promotion = this.data;
        $uibModal.open({
            templateUrl: '/src/views/showPromotion.html',
            controller: 'ShowPromotionController',
            size: 'lg',
            scope: $scope,
            resolve: {
                promotionDetails: promotion
            }
        }).result.then(function () {

        }, function () {
            console.log("Modal dismissed");
        });
    }

    $scope.map = {
        center: [45.4581475, -73.64009765625],
        zoom: 8
    };
    $scope.options = {
        scrollwheel: false
    };

}]);
app.controller('ShowPromotionController', ['$scope', '$uibModalInstance', 'promotionDetails', function ($scope, $uibModalInstance, promotionDetails) {
    //var Category = ["Footwear", "Electronics", "Jewellery", "Restaurants", "Services", "Apparel"];

    $scope.Title = promotionDetails.Title;
    //$scope.Category = Category[promotionDetails.Category];
    $scope.Category = promotionDetails.Category;
    $scope.Description = promotionDetails.Description;
    $scope.PromotionStartDate = promotionDetails.PromotionStartDate;
    $scope.PromotionEndDate = promotionDetails.PromotionEndDate;
    $scope.previewImage = promotionDetails.PromotionImageURL ? "https://glimpseimages.blob.core.windows.net/imagestorage/" + promotionDetails.PromotionImageURL : '';

    $scope.ok = function () {
        $uibModalInstance.close();
    }

}]);
