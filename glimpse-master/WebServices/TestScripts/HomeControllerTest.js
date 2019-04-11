
describe('HomeController', function () {
    var $controller, $rootScope;
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
    }));

    it('Data should be empty initially', function () {
        var scope = $rootScope.$new();
        var controller = $controller('HomeController', { $scope: scope });
        expect(scope.data).toBe('');
    });
});