describe('mapController', function () {
    var $controller, $rootScope, $q, dataService, $scope, getDeferred, queryDeferred;

    var mockPromotionResponse = [{
        "PromotionId": 2085,
        "VendorId": 153,
        "Vendor": null,
        "Title": "Ad odit amet.",
        "Description": "Quisquam cum unde.",
        "Category": 5,
        "PromotionStartDate": "2017-02-07T23:29:33.97",
        "PromotionEndDate": "2017-02-25T23:29:33.97",
        "PromotionImageURL": "153/test/cover",
        "RequestFromWeb": false,
        "PromotionImage": null,
        "PromotionImages": []
    }];

    dataService = {
        getVendors: function () {
            return {
                get: function () {
                    getDeferred = $q.defer();
                    return {
                        $promise: getDeferred.promise
                    };
                }
            }
        },
        getPromotions: function () {
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

    var fakeModal = {
        result: {
            then: function (confirmCallback, cancelCallback) {
                this.confirmCallBack = confirmCallback;
                this.cancelCallback = cancelCallback;
            }
        },
        close: function (item) {
            this.result.confirmCallBack(item);
        },
        dismiss: function (type) {
            this.result.cancelCallback(type);
        }
    };

    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _$q_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        $q = _$q_;
    }));

    beforeEach(angular.mock.inject(function ($controller, _$uibModal_) {
        $scope = $rootScope.$new();

        spyOn(dataService, 'getVendors').and.callThrough();
        spyOn(dataService, 'getPromotions').and.callThrough();

        uibModal = _$uibModal_;

        $controller('mapController', {
            '$scope': $scope,
            'dataService': dataService,
            '$state': {},
            'uibModal': uibModal
        });
    }));


    describe('getPromotions.query', function () {
        it('should call the getPromotions query method', function () {
            queryDeferred.resolve(mockPromotionResponse);
            $rootScope.$apply();
            expect(dataService.getPromotions).toHaveBeenCalled();
        });

        it('should not call the getPromotions query method when getPromotions return error', function () {
            queryDeferred.reject();
            $rootScope.$apply();
            expect(dataService.getPromotions).toHaveBeenCalled();
            expect(dataService.getVendors).not.toHaveBeenCalled();
        });
    });

    describe('getVendors.get', function () {

        beforeEach(function () {
            queryDeferred.resolve(mockPromotionResponse);
            getDeferred.resolve({
                "VendorId": 57,
                "Email": "eerrttyy@gmail.com",
                "Password": "uG0vp2kDVRUjQJ7lzqpUQw==",
                "CompanyName": "jojosam",
                "Salt": "ndyUh1sYPNo=",
                "Address": "350 Bishop, Montreal, QC H3G 2E8, Canada",
                "Telephone": "5146776666",
                "Location": {
                    "Lat": 45.498002499999984,
                    "Lng": -73.57873046875
                },
                "Promotions": []
            });
        });

        it('should call the getVendors get method when getVendors resolve empty object', function () {
            queryDeferred.resolve(mockPromotionResponse);
            getDeferred.resolve({});
            $rootScope.$apply();
            expect(dataService.getVendors).toHaveBeenCalled();
        });

        it('should call the getVendors get method when getVendors resolve result object', function () {
            $scope.promotions = [];
            $rootScope.$apply();
            expect($scope.promotions.length).toEqual(0);
            expect(dataService.getVendors).toHaveBeenCalled();
        });
    });

    describe('Show PromotionData Modal', function () {

        beforeEach(function () {
            modalOptions = {
                templateUrl: '/src/views/showPromotion.html',
                controller: 'ShowPromotionController',
                size: 'lg',
                scope: $scope,
                resolve: {
                    promotionDetails: mockPromotionResponse[0]
                }
            };

            spyOn(uibModal, 'open').and.callFake(function (options) {
                actualOptions = modalOptions;
                return fakeModal;
            });
        });

        it('should call $uibModal.open method when showPromotionData clicked', function () {
            $scope.showPromotionData();
            expect(uibModal.open).toHaveBeenCalled();
        });
    });

});
