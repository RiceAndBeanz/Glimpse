'use strict';

app.controller('HomeController', ['$scope', 'dataService', '$state', 'authenticationService', 'blockUI',function ($scope, dataService, $state, authenticationService, blockUI) {

    $scope.data = "";
    
    dataService.GetAuthorizeData().then(function (data) {
        localStorage.id = data;
        var vendor = dataService.getVendors().get({ vendor: localStorage.id });
        vendor.$promise.then(function (data){
            localStorage.company = vendor.CompanyName;
            localStorage.address = vendor.Address;
            localStorage.email = vendor.Email;
            localStorage.tel = vendor.Telephone;
        }, function(error){
            console.log("vendor not found");
        })
       
    },function (error) {
        console.log("No longer logged in");
        alert("You have been logged out due to session timeout");
        $state.go("login");
    })

    $scope.logout = function () {
        authenticationService.logout();
        $state.go("login");
    }
}]);