

'use strict';

app.controller('ProfileController', ['$scope', 'dataService', '$state', 'authenticationService', function ($scope, dataService, $state, authenticationService) {
    var VendorId;
    $scope.validateAddress = true;
    $scope.data = "";
    $scope.editOn = false;
    var geocoder = new google.maps.Geocoder();
    $scope.location = { lat: '', lng: '' };

    dataService.GetAuthorizeData().then(function (data) {
        VendorId = data;
        console.log(data);
        $scope.email = localStorage.email;
        $scope.company = localStorage.company;
        $scope.address = localStorage.address;
        $scope.tel = localStorage.tel;
        $scope.initEmail = $scope.email;
        $scope.initAddress = $scope.address;
        $scope.initTel = $scope.tel;
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
    $scope.cancel = function () {
        $scope.validateAddress = true;
        $scope.email = $scope.initEmail;
        $scope.address = $scope.initAddress;
        $scope.tel = $scope.initTel;
    }

    $scope.save = function () {
        dataService.getVendors().get({
            vendor: localStorage.id
        }).$promise.then(function (data) {
            geocoder.geocode({ 'address': $scope.address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    $scope.location.lat = results[0].geometry.location.lat();
                    $scope.location.lng = results[0].geometry.location.lng();
                    $scope.validateAddress = true;
                    var location = {
                        Lat: $scope.location.lat,
                        Lng: $scope.location.lng
                    }
                    var vendorInfo = angular.copy(data);
                    vendorInfo["Email"] = $scope.email;
                    vendorInfo["Address"] = $scope.address;
                    vendorInfo["Telephone"] = $scope.tel;
                    vendorInfo["Location"] = location;

                    dataService.updateVendorDetails().update({
                        VendorId: VendorId
                    }, vendorInfo).$promise.then(function (data) {
                        $scope.editOn = false;
                        localStorage.email = $scope.email;
                        localStorage.address = $scope.address;
                        localStorage.tel = $scope.tel
                        $scope.initEmail = $scope.email;
                        $scope.initAddress = $scope.address;
                        $scope.initTel = $scope.tel;
                    }).catch(function (err) {
                        console.log(err);
                    });
                }
                else {
                    $scope.validateAddress = false;
                }
            });
        }, function (error) {
            console.log("vendor not found");
        })
    }
}]);