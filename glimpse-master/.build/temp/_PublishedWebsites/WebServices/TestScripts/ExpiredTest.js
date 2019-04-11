describe('vendorsPromotionsController', function () {
    var $controller, $rootScope, $httpBackend, dataService;


    beforeEach(angular.mock.module('myApp'));
    beforeEach(angular.mock.inject(function ($injector) {
        $httpBackend = $injector.get('$httpBackend');
    }));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _dataService_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        dataService = _dataService_;
    }));



    it('should test expired date', function () {
        var scope = $rootScope.$new();
        var controller = $controller('vendorsPromotionsController', {
            $scope: scope,
            $state: {},
            $uibModal: {}
        });


        expect(scope.isPromotionExpired(new Date("01-02-2015"))).toBe(true);
        expect(scope.isPromotionExpired(new Date("21-02-2018"))).toBe(false);
    });


    it('should have defined editPromotion method', function () {
        var scope = $rootScope.$new();
        var controller = $controller('vendorsPromotionsController', {
            $scope: scope,
            $state: {},
            $uibModal: {}
        });
        expect(scope.editPromotion).toBeDefined();
    });

    it('should have defined deletePromotion method', function () {
        var scope = $rootScope.$new();
        var controller = $controller('vendorsPromotionsController', {
            $scope: scope,
            $state: {},
            $uibModal: {}
        });
        expect(scope.deletePromotion).toBeDefined();
    });

    it('should call deletePromotion method', function () {
        var scope = $rootScope.$new();
        var controller = $controller('vendorsPromotionsController', {
            $scope: scope,
            $state: {},
            $uibModal: {}
        });
        var spy = spyOn(scope, 'deletePromotion');

        var promotion = { "PromotionId": 3 };
        scope.deletePromotion(promotion, 2);
        expect(scope.deletePromotion).toBeDefined();
        expect(spy).toHaveBeenCalled();

    });

    it('should call editPromotion method', function () {
        var scope = $rootScope.$new();
        var controller = $controller('vendorsPromotionsController', {
            $scope: scope,
            $state: {},
            $uibModal: {}
        });
        var spy = spyOn(scope, 'editPromotion');

        var promotion = { "PromotionId": 1 };
        scope.editPromotion(promotion);
        expect(scope.editPromotion).toBeDefined();
        expect(spy).toHaveBeenCalled();

    });
});
