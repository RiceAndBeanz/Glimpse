'use strict';

app.controller('LoginController', ['$scope', '$http', 'blockUI', 'authenticationService', '$state', '$timeout', function ($scope, $http, blockUI, authenticationService, $state, $timeout) {
    
    $scope.preventLogin;
    $scope.login = function () {
        $scope.preventLogin = true;
        
        authenticationService.login($scope.user).then(function (data) {
            $state.go("home.viewPromotion");
        }, function (error) {
            $scope.message = error.error_description;
            console.log($scope.message);
            $scope.incorrect = true;
        })

        $timeout(function () {
            $scope.preventLogin = false;
        },3000)
    }

}]);
