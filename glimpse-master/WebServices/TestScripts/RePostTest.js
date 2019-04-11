describe('changeDateModalController', function () {
    var $controller, $rootScope, promotionDetails;
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
    }));

    it('should be able to show previous date', function () {
        var scope = $rootScope.$new();
        promotionDetails = {
            "PromotionStartDate": (new Date("09-12-2016")).getTime(),
            "PromotionEndDate": (new Date("01-01-2017")).getTime()
        }
        var controller = $controller('changeDateModalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: promotionDetails });
        scope.sdt = promotionDetails.PromotionStartDate;
        scope.edt = promotionDetails.PromotionEndDate;
        var startDate = (new Date("09-12-2016")).getTime();
        var endDate = (new Date("01-01-2017")).getTime();
        expect(scope.sdt).toBe(startDate);
        expect(scope.edt).toBe(endDate);
    });

    it('should toggle showDateWarning when start date is smaller then end date', function () {
        var scope = $rootScope.$new();
        promotionDetails = {
            "PromotionStartDate": new Date("01-01-2017"),
            "PromotionEndDate": new Date("09-12-2016")
        }
        var controller = $controller('changeDateModalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: promotionDetails });
        scope.sdt = promotionDetails.PromotionStartDate;
        scope.edt = promotionDetails.PromotionEndDate;
        scope.done();
        expect(scope.showDateWarning).toBe(true);
    });
});
