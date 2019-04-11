app.factory('dataService', ['$http', '$resource', function ($http, $resource) {
    var fac = {};
    fac.GetAuthorizeData = function () {
        return $http.get('/api/data/home').then(function (response) {
            return response.data;
        })
    }
    fac.getVendors = function () {
        return $resource('/api/vendors/:vendor', { user: "@vendor" });
    }
    fac.updateVendorDetails = function () {
        return $resource('/api/vendors/:VendorId', null, {
            'update': {
                method: 'PUT'
            }
        });
    }
    fac.getPromotions = function () {
        return $resource('/api/promotions/:promotion', { user: "@promotion" });
    }
    fac.savePromotionImages = function (imageInfo) {
        return $http.post('/api/PromotionImages', imageInfo);
    }
    fac.updatePromotion = function () {
        return $resource('/api/promotions/:promotion', null, {
            'update': {
                method: 'PUT'
            }
        });
    }
    fac.deletePromotion = function () {
        return $resource('/api/promotions/:promotion', null, {
            'delete': {
                method: 'DELETE'
            }
        });
    }
    fac.getAllPromotionFromSpecificVendor = function (vendorId) {
        return $resource('/api/vendors/'+vendorId+'/promotions', { user: "@promotion" })
    }
    fac.getPromotionClicks = function () {
        return $resource('/api/promotionclicks', { user: "@promotionclicks" })
    }
    fac.getPromotionImagesFromSpecificPromotion = function (promoId) {
        return $resource('/api/Promotions/'+promoId+'/promotionimages', { user: "@promotionimages" })
    }
    return fac;
}])
