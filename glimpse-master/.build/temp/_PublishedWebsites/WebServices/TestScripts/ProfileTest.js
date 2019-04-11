describe('ProfileController', function () {
    var $controller, $rootScope, $q, dataService, $scope, getDeferred, getAuthorizedataDeferred, updateDeferred;

    dataService = {
        GetAuthorizeData: function () {
            getAuthorizedataDeferred = $q.defer();
            return getAuthorizedataDeferred.promise;
        },
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
        updateVendorDetails: function () {
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

    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _$q_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        $q = _$q_;
    }));

    beforeEach(angular.mock.inject(function ($controller) {
        $scope = $rootScope.$new();

        spyOn(dataService, 'GetAuthorizeData').and.callThrough();
        spyOn(dataService, 'getVendors').and.callThrough();
        spyOn(dataService, 'updateVendorDetails').and.callThrough();

        $controller('ProfileController', {
            '$scope': $scope,
            'dataService': dataService,
            '$state': {},
            'authenticationService': {}
        });
    }));


    describe('GetAuthorizeData.query', function () {

        it('should call the GetAuthorizeData method', function () {
            expect(dataService.GetAuthorizeData).toHaveBeenCalled();
        });
    });

    describe('getVendors.get', function () {

        it('should call the getVendors method', function () {
            $scope.save();
            getDeferred.resolve({});
            expect(dataService.getVendors).toHaveBeenCalled();
        });
    });

    describe('updateVendorDetails.update', function () {

        it('should call the getVendors method', function () {
            $scope.save();
            getDeferred.resolve({});
            $rootScope.$apply();
            expect(dataService.getVendors).toHaveBeenCalled();
            expect(dataService.updateVendorDetails).toHaveBeenCalled();
        });
    });

    describe('initial variable settings of profile controller', function () {

        it('should have editOn set as false initially', function () {
            expect($scope.editOn).toBe(false);
        });

        it('should have editOn set as true when edit method called ', function () {
            $scope.edit();
            expect($scope.editOn).toBe(true);
        });
    });
});


