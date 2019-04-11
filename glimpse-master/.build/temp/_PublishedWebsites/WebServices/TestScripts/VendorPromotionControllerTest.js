describe('vendorsPromotionsController', function () {
    var $controller, $rootScope, $q, dataService, $scope, queryDeferred, deleteDeferred, updateDeferred, uibModal, uibModalDeferred, actualOptions, modalOptions;

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

    var updatedPromotionMockResponse = {
        "PromotionId": 2085,
        "VendorId": 153,
        "Vendor": null,
        "Title": "Ad odit.",
        "Description": "Quisquam",
        "Category": 5,
        "PromotionStartDate": "2017-02-07T23:29:33.97",
        "PromotionEndDate": "2017-02-25T23:29:33.97",
        "PromotionImageURL": "153/test/cover",
        "RequestFromWeb": false,
        "PromotionImage": null,
        "PromotionImages": []
    };

    dataService = {
        GetAuthorizeData: function () {
            return $q.defer().promise;
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
        },
        deletePromotion: function () {
            return {
                delete: function () {
                    deleteDeferred = $q.defer();
                    return {
                        $promise: deleteDeferred.promise
                    };
                }
            }
        },
        updatePromotion: function () {
            return {
                update: function () {
                    updateDeferred = $q.defer();
                    return {
                        $promise: updateDeferred.promise
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

        spyOn(dataService, 'getAllPromotionFromSpecificVendor').and.callThrough();
        spyOn(dataService, 'GetAuthorizeData').and.callThrough();
        spyOn(dataService, 'deletePromotion').and.callThrough();
        spyOn(dataService, 'updatePromotion').and.callThrough();

        uibModal = _$uibModal_;

        $controller('vendorsPromotionsController', {
            '$scope': $scope,
            'dataService': dataService,
            '$state': {},
            '$uibModal': uibModal
        });
    }));


    describe('getAllPromotionFromSpecificVendor.query', function () {

        it('should call the GetAuthorizeData method', function () {
            expect(dataService.GetAuthorizeData).toHaveBeenCalled();
        });

        it('should call the getAllPromotionFromSpecificVendor', function () {
            expect(dataService.getAllPromotionFromSpecificVendor).toHaveBeenCalled();
        });

        it('should set the response from the getAllPromotionFromSpecificVendor to $scope.mypromotions', function () {
            queryDeferred.resolve(mockPromotionResponse);
            $rootScope.$apply();
            expect($scope.mypromotions).toEqual(mockPromotionResponse);
        });

        it('should not set the response from the getAllPromotionFromSpecificVendor to $scope.mypromotions', function () {
            queryDeferred.reject(mockPromotionResponse);
            $rootScope.$apply();
            expect($scope.mypromotions).not.toEqual(mockPromotionResponse);
        });
    });

    describe('In Delete Promotion Test', function () {

        beforeEach(function () {
            $scope.mypromotions = mockPromotionResponse;
            $rootScope.$apply();
        });

        it('should call the deletePromotion method of dataService', function () {
            $scope.deletePromotion(mockPromotionResponse[0], 0);
            expect(dataService.deletePromotion).toHaveBeenCalled();
        });

        it('should not delete promotion when dataService.deletePromotion fail', function () {
            $scope.deletePromotion(mockPromotionResponse[0], 0);
            deleteDeferred.reject();
            $rootScope.$apply();
            expect($scope.mypromotions.length).toEqual(1);
        });

        it('should able to delete promotion from promotion list', function () {
            $scope.deletePromotion(mockPromotionResponse[0], 0);
            deleteDeferred.resolve();
            $rootScope.$apply();
            expect($scope.mypromotions.length).toEqual(0);
        });
    });

    describe('Edit Promotion', function () {

        beforeEach(function () {
            spyOn(uibModal, 'open').and.returnValue(fakeModal);
            $scope.mypromotions = mockPromotionResponse;
            $rootScope.$apply();
        });

        it('should call $uibModal.open method', function () {
            $scope.editPromotion($scope.mypromotions[0]);
            expect(uibModal.open).toHaveBeenCalled();
        });
    });

    describe('Show Edit Promotiondate Modal', function () {

        beforeEach(function () {
            $scope.mypromotions = mockPromotionResponse;
            $rootScope.$apply();

            modalOptions = {
                templateUrl: '/src/views/changePromotionDate.html',
                controller: 'changeDateModalController',
                size: 'lg',
                scope: $scope,
                resolve: {
                    promotionDetails: $scope.mypromotions[0]
                }
            };

            spyOn(uibModal, 'open').and.callFake(function (options) {
                actualOptions = modalOptions;
                return fakeModal;
            });
        });

        it('should call $uibModal.open method when editPromotionDate clicked', function () {
            $scope.editPromotionDate($scope.mypromotions[0]);
            expect(uibModal.open).toHaveBeenCalled();
        });

        it('should call $uibModal.open method with assigend templateUrl and other options', function () {
            $scope.editPromotionDate($scope.mypromotions[0]);
            expect(uibModal.open).toHaveBeenCalledWith(modalOptions);
        });

    });


    describe('Show Edit PromotionDate Modal', function () {

        beforeEach(inject(function ($controller, $rootScope) {
            $scope.mypromotions = [{
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
            $rootScope.$apply();
            spyOn(uibModal, 'open').and.returnValue(fakeModal);
        }));


        // Initialize the controller and a mock scope
        beforeEach(inject(function ($controller, $rootScope) {
            scope = $rootScope.$new();
            modalControllerMock = $controller('changeDateModalController', {
                $scope: scope,
                $uibModalInstance: fakeModal,
                promotionDetails: {}
            });
        }));


        it('should call $uibModal.open method when editPromotionDate clicked', function () {
            $scope.editPromotionDate($scope.mypromotions[0]);
            expect(uibModal.open).toHaveBeenCalled();
        });

        it('should call dataService.updatePromotion when user click okay', function () {

            $scope.editPromotionDate({
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
            });

            fakeModal.close({
                startDate: "2017-03-07T23:29:33.97",
                endDate: "2017-04-07T23:29:33.97"
            });
            expect(dataService.updatePromotion).toHaveBeenCalled();
        });

        it('should update promotion date', function () {

            $scope.editPromotionDate({
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
            });

            fakeModal.close({
                startDate: "2017-03-07T23:29:33.97",
                endDate: "2017-04-07T23:29:33.97"
            });

            updateDeferred.resolve({
                "PromotionId": 2085,
                "VendorId": 153,
                "Vendor": null,
                "Title": "Ad odit amet.",
                "Description": "Quisquam cum unde.",
                "Category": 5,
                "PromotionStartDate": "2017-03-07T23:29:33.97",
                "PromotionEndDate": "2017-04-07T23:29:33.97",
                "PromotionImageURL": "153/test/cover",
                "RequestFromWeb": false,
                "PromotionImage": null,
                "PromotionImages": []
            });
            $rootScope.$apply();
            expect($scope.mypromotions[0]).toEqual({
                "PromotionId": 2085,
                "VendorId": 153,
                "Vendor": null,
                "Title": "Ad odit amet.",
                "Description": "Quisquam cum unde.",
                "Category": 5,
                "PromotionStartDate": "2017-03-07T23:29:33.97",
                "PromotionEndDate": "2017-04-07T23:29:33.97",
                "PromotionImageURL": "153/test/cover",
                "RequestFromWeb": false,
                "PromotionImage": null,
                "PromotionImages": []
            });
        });

    });
});



