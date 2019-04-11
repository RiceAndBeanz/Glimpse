describe('modalController', function () {
    var $controller, $rootScope, $q, dataService, $scope, queryDeferred, updateDeferred, uibModal, uibModalDeferred, actualOptions, modalOptions;
    var $timeout, $http;

    Caman = function () {

    };

    var promotionDetails = {
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
        "PromotionImages": ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"]
    };

    dataService = {
        getPromotions: function () {
            return {
                save: function () {
                    queryDeferred = $q.defer();
                    return queryDeferred.promise
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

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _$q_, _$timeout_, _$http_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        $q = _$q_;
        $timeout = _$timeout_;
        $http = _$http_;
    }));

    beforeEach(angular.mock.inject(function ($controller, _$uibModal_) {
        $scope = $rootScope.$new();

        spyOn(dataService, 'getPromotions').and.callThrough();
        spyOn(dataService, 'updatePromotion').and.callThrough();

        uibModal = _$uibModal_;

        $controller('modalController', {
            '$scope': $scope,
            '$uibModalInstance': {},
            'Upload': {},
            '$timeout': $timeout,
            'dataService': dataService,
            '$http': $http,
            'promotionDetails': promotionDetails,
            '$q': $q,
            'edit': {}
        });
    }));


    describe('Filter Logic', function () {

        it('should able to have default selectedFilter value', function () {
            expect($scope.selectedFilter).toEqual('Apply Filters');
        });

        it('should able to apply Vintage filter', function () {
            $scope.applyFilter('Vintage');
            expect($scope.selectedFilter).toEqual('Vintage');
        });

        it('should have slider filter disable when filter applied', function () {
            $scope.applyFilter('Vintage');
            $rootScope.$apply();
            expect($scope.isSilderFilterEnable).toEqual(false);
        });

        it('should able to apply Lomo filter', function () {
            $scope.applyFilter('Lomo');
            expect($scope.selectedFilter).toEqual('Lomo');
        });

        it('should able to apply Clarity filter', function () {
            $scope.applyFilter('Clarity');
            expect($scope.selectedFilter).toEqual('Clarity');
        });

        it('should able to apply SinCity filter', function () {
            $scope.applyFilter('SinCity');
            expect($scope.selectedFilter).toEqual('SinCity');
        });


        it('should able to apply Sunrise filter', function () {
            $scope.applyFilter('Sunrise');
            expect($scope.selectedFilter).toEqual('Sunrise');
        });

        it('should able to apply CrossProcess filter', function () {
            $scope.applyFilter('CrossProcess');
            expect($scope.selectedFilter).toEqual('CrossProcess');
        });

        it('should able to apply OrangePeel filter', function () {
            $scope.applyFilter('OrangePeel');
            expect($scope.selectedFilter).toEqual('OrangePeel');
        });

        it('should able to apply Grungy filter', function () {
            $scope.applyFilter('Grungy');
            expect($scope.selectedFilter).toEqual('Grungy');
        });

        it('should able to apply Jarques filter', function () {
            $scope.applyFilter('Jarques');
            expect($scope.selectedFilter).toEqual('Jarques');
        });

        it('should able to apply Pinhole filter', function () {
            $scope.applyFilter('Pinhole');
            expect($scope.selectedFilter).toEqual('Pinhole');
        });


        it('should able to apply OldBoot filter', function () {
            $scope.applyFilter('OldBoot');
            expect($scope.selectedFilter).toEqual('OldBoot');
        });

        it('should able to apply GlowingSun filter', function () {
            $scope.applyFilter('GlowingSun');
            expect($scope.selectedFilter).toEqual('GlowingSun');
        });

        it('should able to apply HazyDays filter', function () {
            $scope.applyFilter('HazyDays');
            expect($scope.selectedFilter).toEqual('HazyDays');
        });

        it('should able to apply Nostalgia filter', function () {
            $scope.applyFilter('Nostalgia');
            expect($scope.selectedFilter).toEqual('Nostalgia');
        });

        it('should able to apply Hemingway filter', function () {
            $scope.applyFilter('Hemingway');
            expect($scope.selectedFilter).toEqual('Hemingway');
        });

        it('should able to apply Concentrate filter', function () {
            $scope.applyFilter('Concentrate');
            expect($scope.selectedFilter).toEqual('Concentrate');
        });

        it('should able to apply HerMajesty filter', function () {
            $scope.applyFilter('HerMajesty');
            expect($scope.selectedFilter).toEqual('HerMajesty');
        });


        it('should have slider filter disable when filter applied', function () {
            $scope.applyFilter('Vintage');
            $rootScope.$apply();
            expect($scope.isSilderFilterEnable).toEqual(false);
        });

        it('should able to reset filter', function () {
            $scope.applyFilter('Custom Filter');
            expect($scope.brightness.value).toEqual(0);
            expect($scope.contrast.value).toEqual(0);
            expect($scope.vibrance.value).toEqual(0);
            expect($scope.saturation.value).toEqual(0);
            expect($scope.hue.value).toEqual(0);
            expect($scope.exposure.value).toEqual(0);
            expect($scope.sepia.value).toEqual(0);
            expect($scope.clip.value).toEqual(0);
            expect($scope.noise.value).toEqual(0);
            expect($scope.stackblur.value).toEqual(0);
            expect($scope.sharpen.value).toEqual(0);
        });
    });



    describe('Slider Logic', function () {
        beforeEach(function () {
            $scope.slides = ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"];
            $rootScope.$apply();
        });

        it('should able to set direction right when index is lesser then current index', function () {
            $scope.currentIndex = 3;
            $scope.setCurrentSlideIndex(2);
            $rootScope.$apply();
            expect($scope.direction).toEqual('right');
            expect($scope.currentIndex).toEqual(2);
        });

        it('should able to set direction left when index is lesser then current index', function () {
            $scope.currentIndex = 3;
            $scope.setCurrentSlideIndex(4);
            $rootScope.$apply();
            expect($scope.direction).toEqual('left');
            expect($scope.currentIndex).toEqual(4);
        });

        it('should able check whether current index is equal or not', function () {
            $scope.currentIndex = 3;
            $rootScope.$apply();
            expect($scope.isCurrentSlideIndex(3)).toEqual(true);
        });

        it('should able navigate to prev image', function () {
            $scope.slides = ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"];
            $scope.prevSlide();
            $rootScope.$apply();
            expect($scope.direction).toEqual('left');
            expect($scope.currentIndex).toEqual(1);
        });

        it('should able navigate to next image', function () {
            $scope.slides = ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"];
            $scope.nextSlide();
            $rootScope.$apply();
            expect($scope.direction).toEqual('right');
            expect($scope.currentIndex).toEqual(2);
        });
    });
});