'use strict';

app.controller('SignupController', ['$scope', '$http', 'dataService', '$state', function ($scope, $http, dataService, $state) {
    $scope.passConfirmation = false;
    $scope.userTaken = false;
    $scope.location = { lat: '', lng: '' };
    $scope.user = { streetnumber: '', streetname: '', postal: '', city: '', province: '', country: '' };
    $scope.addressIsValid = true;
    var geocoder = new google.maps.Geocoder();

    $scope.createUser = function () {
        var location = {
            Lat: $scope.location.lat,
            Lng: $scope.location.lng
        }
        var userData = {
            email: $scope.user.email,
            companyName: $scope.user.company,
            password: $scope.user.password,
            address: $scope.address,
            telephone: $scope.user.personalphone,
            Location: location,
            RequestFromWeb : true
        }
        if ($scope.address != false) {
            $scope.passConfirmation = false;
            dataService.getVendors().save(userData, function (resp, headers) {
                $state.go("login");
            },
            function (err) {
                $scope.userTaken = true;
            });
        }
        else {
            console.log("Form is invalid");
        }
    }

    var getAddress = function () {
        if ($scope.user.streetnumber != '' && $scope.user.streetname != '' && $scope.user.postal != '' && $scope.user.city != '' && $scope.user.province != '' && $scope.user.country != '') {
            return $scope.user.streetnumber + ", " + $scope.user.streetname + ", " + $scope.user.postal + ", " + $scope.user.city + ", " + $scope.user.province + ", " + $scope.user.country;
        }
        else
            return false;
    }

    $scope.validateAddress = function () {
        $scope.address = getAddress();
        if ($scope.user.password == $scope.user.confirmpassword) {
            $scope.passConfirmation = false;
            geocoder.geocode({ 'address': $scope.address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    $scope.addressIsValid = true;
                    $scope.location.lat = results[0].geometry.location.lat();
                    $scope.location.lng = results[0].geometry.location.lng();
                    $scope.createUser();
                }
                else {
                    $scope.addressIsValid = false;
                    return false;
                }
            });
        }
        else {
            $scope.passConfirmation = true;
        }
    }
}]);
