

'use strict';

app.controller('ProfileController', ['$scope', 'dataService', '$state', 'authenticationService', function ($scope, dataService, $state, authenticationService) {
    var VendorId;
    $scope.data = "";
    $scope.editOn = false;
    dataService.GetAuthorizeData().then(function (data) {
        debugger;
        VendorId = data;
        console.log(data);
        $scope.email = localStorage.email;
        $scope.company = localStorage.company;
        $scope.address = localStorage.address;
        $scope.tel = localStorage.tel;
    },function (error) {
        console.log("No longer logged in");
        alert("You have been logged out due to session timeout");
    })

    $scope.edit = function () {
        if ($scope.editOn == true) {
            $scope.editOn = false;
        } else {
            $scope.editOn = true;
        }
      
    }
    $scope.save = function () {
      

        dataService.getVendors().get({
            vendor: localStorage.id
        }).$promise.then(function (data) {

            var vendorInfo = angular.copy(data);
            vendorInfo["Email"] = $scope.email;
            vendorInfo["Address"] = $scope.address;
            vendorInfo["Telephone"] = $scope.tel;

            dataService.updateVendorDetails().update({
                VendorId: VendorId
            }, vendorInfo).$promise.then(function (data) {
                $scope.editOn = false;
                localStorage.email = $scope.email;
                localStorage.address = $scope.address;
                localStorage.tel = $scope.tel
            }).catch(function (err) {
                console.log(err);
            });
        }, function (error) {
            console.log("vendor not found");
        })
    }

}]);