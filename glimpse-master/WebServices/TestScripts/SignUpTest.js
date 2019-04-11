
describe('SignupController', function () {
    var $controller, $rootScope;
    beforeEach(angular.mock.module('myApp'));

    beforeEach(angular.mock.inject(function (_$controller_, _$rootScope_) {
        $controller = _$controller_;
        $rootScope = _$rootScope_;
    }));

    it('should have the user information empty initially', function () {

        var scope = $rootScope.$new();
        var controller = $controller('SignupController', { $scope: scope });
        expect(scope.user.streetnumber).toBe('');
        expect(scope.user.streetname).toBe('');
        expect(scope.user.postal).toBe('');
        expect(scope.user.city).toBe('');
        expect(scope.user.province).toBe('');
        expect(scope.user.country).toBe('');
    });

    it('should initialize the scope value', function () {

        var scope = $rootScope.$new();
        var controller = $controller('SignupController', { $scope: scope });
        scope.user = {};
        scope.user.postal = '12345';
        scope.user.city = 'New York';
        scope.user.country = 'USA'
        expect(scope.user).not.toBe(undefined);
        expect(scope.user.country).toBe('USA');
        expect(scope.user.city).toBe('New York');
        expect(scope.user.postal).toBe('12345');
    });
});