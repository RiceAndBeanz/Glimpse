'use strict';

app.controller('PromotionController', ['$scope', 'dataService', '$state', '$uibModal', function ($scope, dataService, $state, $uibModal) {
    $scope.data = "";

    dataService.GetAuthorizeData().then(function (data) {
        console.log("Authorized");
    }, function (error) {
        console.log("No longer logged in");
        alert("You have been logged out due to session timeout");
    })

    var promotionsquery = dataService.getPromotions().query();
    promotionsquery.$promise.then(function (data) {
        //debugger;
        $scope.promotions = data;
    }, function (error) {
        console.log("Error: Could not load promotions");
    })

    $scope.isPromotionExpired = function (promotionEndDate) {
        var currentDate = new Date();
        var promotionEndDate = new Date(promotionEndDate);
        return currentDate.getTime() > promotionEndDate.getTime();
    }

    $scope.showImage = function (promotion) {
        $uibModal.open({
            templateUrl: '/src/views/imageFullSizeView.html',
            controller: 'modalImageController',
            size: 'lg',
            scope: $scope,
            resolve: {
                promotionDetails: promotion
            }
        }).result.then(function (result) {
            console.log("Modal dismissed");
        }, function () {
            console.log("Modal dismissed");
        });
    }

   

}]);

app.controller('modalImageController', function ($scope, $timeout, dataService, promotionDetails) {
    
    $scope.promotionImageURL = promotionDetails.PromotionImageURL;
    $scope.promotionImages = [];
    $scope.images = [];
    $scope.images[0] = $scope.promotionImageURL;
    
    var promotionsquery = dataService.getPromotionImagesFromSpecificPromotion(promotionDetails.PromotionId).query();
    promotionsquery.$promise.then(function (data) {
        $scope.promotionImages = data;
        getImages();
    }, function (error) {})

    var getImages = function () {
        for (var i = 0; i < $scope.promotionImages.length; i++) {
            $scope.images.push($scope.promotionImages[i].ImageURL);
        }
    }
    $scope.direction = 'left';
    $scope.currentIndex = 0;

    $scope.setCurrentSlideIndex = function (index) {
        $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
        $scope.currentIndex = index;
    };

    $scope.isCurrentSlideIndex = function (index) {
        return $scope.currentIndex === index;
    };

    $scope.prevSlide = function () {
        $scope.direction = 'left';
        $scope.currentIndex = ($scope.currentIndex < $scope.images.length - 1) ? ++$scope.currentIndex : 0;
    };

    $scope.nextSlide = function () {
        $scope.direction = 'right';
        $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.images.length - 1;
    };

});