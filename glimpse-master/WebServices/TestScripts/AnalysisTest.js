describe('Analysis Controller', function () {
    var $controller, $rootScope, $q, dataService, $scope, queryDeferred, updateDeferred;

    var mockPromotionResponse = [{
        "PromotionClickId": 469,
        "PromotionId": 2128,
        "Time": "2017-02-21T21:15:47.66"
    }];

    dataService = {
        getPromotionClicks: function () {
            return {
                query: function () {
                    queryDeferred = $q.defer();
                    return {
                        $promise: queryDeferred.promise
                    };
                }
            }
        },
        getAllPromotionFromSpecificVendor: function () {
            return {
                query: function () {
                    queryDeferred = $q.defer();
                    return {
                        $promise: queryDeferred.promise
                    };
                }
            }
        }
    };
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _$q_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        $q = _$q_;

        $scope = $rootScope.$new();

        spyOn(dataService, 'getPromotionClicks').and.callThrough();
        spyOn(dataService, 'getAllPromotionFromSpecificVendor').and.callThrough();


        $controller('analysisController', {
            '$scope': $scope,
            'dataService': dataService
        });

    }));

    it('should have the noPromotionClicked to set as false initially', function () {
        expect($scope.noPromotionClicked).toBe(false);
    });

    describe('getAllPromotionFromSpecificVendor.query', function () {
        beforeEach(function () {
            queryDeferred.resolve({});
            $rootScope.$apply();
        });

        it('should call the getAllPromotionFromSpecificVendor', function () {
            expect(dataService.getAllPromotionFromSpecificVendor).toHaveBeenCalled();
        });
    });


    describe('getPromotionClicks.query', function () {
        beforeEach(function () {
            queryDeferred.resolve({});
            $rootScope.$apply();
        });

        it('should call the getPromotionClicks', function () {
            expect(dataService.getPromotionClicks).toHaveBeenCalled();
        });

        it('should have the labels to be defined', function () {
            expect($scope.labels).toBeDefined();
        });

        it('should have the series to be defined', function () {
            expect($scope.series).toBeDefined();
        });

        it('should have the seriesTitle to be defined', function () {
            expect($scope.seriesTitle).toBeDefined();
        });

        it('should have the promotionClicks toBeUndefined initially', function () {
            expect($scope.promotionClicks).toBeUndefined();
        });

    });
});