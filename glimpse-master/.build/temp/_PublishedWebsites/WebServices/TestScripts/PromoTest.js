
describe('modalController', function () {
    var $controller, $rootScope;
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
    }));

    it('should have the reset filter set as false initially', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        expect(scope.isResetEnable).toBe(false);
    });

    it('should set crop enable as false initially', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        expect(scope.isCropImageEnable).toBe(false);
    });

    it('should have saveCrop set as false initially', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        expect(scope.saveCrop).toBe(false);
    });

    it('should be able to call cropImage method', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        var spy = spyOn(scope, 'cropImage');
        scope.cropImage();
        expect(scope.cropImage).toBeDefined();
        expect(spy).toHaveBeenCalled();
    });

    it('should have isCropImageEnable and saveCrop disabled when the cancelCrop method is called', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        scope.cancelCrop();
        expect(scope.saveCrop).toBe(false);
        expect(scope.isCropImageEnable).toBe(false);
    });

    it('should have clear image and hide crop preview when the removeImage method is called', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {}, edit: {} });
        scope.removeImage();
        expect(scope.picFile).toBe(null);
        expect(scope.croppedDataUrl).toBe(null);
        expect(scope.picFile).toBe(null);
        expect(scope.previewImage).toBe('');
        expect(scope.isCropImageEnable).toBe(false);
    });
    /*it('should have apply filter initially set to selectedFilter and when calling applyfilter its value should change to the selected parameter', function () {
        var scope = $rootScope.$new();
        var controller = $controller('modalController', { $scope: scope, $uibModalInstance: {}, promotionDetails: {} });
        expect(scope.selectedFilter).toBe("Apply Filters");
        scope.applyFilter('Greyscale');
        expect(scope.selectedFilter).toBe("Greyscale");
    });*/
});