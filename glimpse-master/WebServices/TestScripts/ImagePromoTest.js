describe('modalImageController Controller', function () {
    var $controller, $rootScope, $q, dataService, $scope, queryDeferred, $timeout;

    dataService = {
        getPromotionImagesFromSpecificPromotion: function() {
            return {
                query: function() {
                    queryDeferred = $q.defer();
                    return {
                        $promise: queryDeferred.promise
                    };
                }
            }
        }
    };
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_, _$q_, _$timeout_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
        $q = _$q_;
        $timeout = _$timeout_;

        $scope = $rootScope.$new();
        
        spyOn(dataService, 'getPromotionImagesFromSpecificPromotion').and.callThrough();
        

        $controller('modalImageController', {
            '$scope': $scope,
            '$timeout': $timeout,
            'dataService': dataService,
            'promotionDetails': {
                "PromotionImageURL": ["imageurl1.jpeg", "imageurl2.jpeg"]
            }

        });

    }));

    describe('getPromotionImagesFromSpecificPromotion.query', function (){
        beforeEach(function() {
            queryDeferred.resolve(["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"]);
            $rootScope.$apply();
        });

        it('should call the getPromotionImagesFromSpecificPromotion', function() {
             expect(dataService.getPromotionImagesFromSpecificPromotion).toHaveBeenCalled();
        });

        it('should able to assign image list to promotionImages', function() {
             expect($scope.promotionImages).toEqual(["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"]);
        });


        it('should have length of image 4', function() {
             expect($scope.images.length).toEqual(4);
        });
    });

    describe('Slider Logic', function (){
        beforeEach(function() {
            queryDeferred.resolve(["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"]);
            $rootScope.$apply();
        });

        it('should able to set direction right when index is lesser then current index', function() {
            $scope.currentIndex = 3;
            $scope.setCurrentSlideIndex(2);
            $rootScope.$apply();
            expect($scope.direction).toEqual('right');
            expect($scope.currentIndex).toEqual(2);
        });

        it('should able to set direction left when index is lesser then current index', function() {
            $scope.currentIndex = 3;
            $scope.setCurrentSlideIndex(4);
            $rootScope.$apply();
            expect($scope.direction).toEqual('left');
            expect($scope.currentIndex).toEqual(4);
        });

        it('should able check whether current index is equal or not', function() {
            $scope.currentIndex = 3;
            $rootScope.$apply();
            expect($scope.isCurrentSlideIndex(3)).toEqual(true);
        });

        it('should able navigate to prev image', function() {
            $scope.images = ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"];
            $scope.prevSlide();
            $rootScope.$apply();
            expect($scope.direction).toEqual('left');
            expect($scope.currentIndex).toEqual(1);
        });

        it('should able navigate to next image', function() {
            $scope.images = ["imageurl1.jpeg", "imageurl2.jpeg", "imageurl3.jpeg"];
            $scope.nextSlide();
            $rootScope.$apply();
            expect($scope.direction).toEqual('right');
            expect($scope.currentIndex).toEqual(2);
        });
    });


    describe('Should defined all scope variable', function (){

        it('should have the promotionImageURL defined', function() {
            $rootScope.$apply();
            expect($scope.promotionImageURL).toBeDefined();
        });

        it('should have the promotionImages defined', function() {
            $rootScope.$apply();
            expect($scope.promotionImages).toBeDefined();
        });

        it('should have the promotionImages defined', function() {
            $rootScope.$apply();
            expect($scope.images).toBeDefined();
        });


        it('should have the promotionImages defined', function() {
            $rootScope.$apply();
            expect($scope.images[0]).toBeDefined();
        });

        it('should have the direction defined', function() {
            $rootScope.$apply();
            expect($scope.direction).toBeDefined();
        });

        it('should have the currentIndex defined', function() {
            $rootScope.$apply();
            expect($scope.currentIndex).toBeDefined();
        });

    });

    describe('Should able to set initial scope variable', function (){

        it('should able to assign promotionImageURL value', function() {
            $rootScope.$apply();
            expect($scope.promotionImageURL).toEqual(["imageurl1.jpeg", "imageurl2.jpeg"]);
        });

        it('should have the promotionImages length 0', function() {
            $rootScope.$apply();
            expect($scope.promotionImages.length).toEqual(0);
        });

        it('should have the images length 1', function() {
            $rootScope.$apply();
            expect($scope.images.length).toEqual(1);
        });

        it('should have the direction default value left', function() {
            $rootScope.$apply();
            expect($scope.direction).toEqual('left');
        });

        it('should have the currentIndex default value', function() {
            $rootScope.$apply();
            expect($scope.currentIndex).toEqual(0);
        });
    });
});