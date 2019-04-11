'use strict';

var app = angular.module('myApp', ['ui.router', 'ngRoute', 'ui.bootstrap', 'ngResource', 'blockUI', 'LocalStorageModule', 'ngFileUpload', 'ngMap', 'chart.js', 'ngAnimate', 'rzModule']);

app.config(function ($stateProvider, $urlRouterProvider, $qProvider, $locationProvider) {

    $qProvider.errorOnUnhandledRejections(false);
    $urlRouterProvider.otherwise('/login');
    $stateProvider
        .state('login', {
            url: '/login',
            controller: 'LoginController',
            templateUrl: 'src/views/loginView.html'
        })
        .state('signup', {
            url: '/signup',
            controller: 'SignupController',
            templateUrl: 'src/views/signupView.html'
        })
        .state('home', {
            url: '/home',
            controller: 'HomeController',
            templateUrl: 'src/views/homeView.html',
            resolve: {
                "check": function (userService, $state) {
                    if (userService.CurrentUser == null) {
                        $state.go("login");
                    }
                }
            }
        })
        .state('home.viewPromotion', {
            url: '/promotions',
            controller: 'PromotionController',
            templateUrl: 'src/views/viewPromotion.html'
        })
        .state('home.profile', {
            url: '/profile',
            controller: 'ProfileController',
            templateUrl: 'src/views/profileView.html'
        })
        .state('home.map', {
            url: '/map',
            controller: 'mapController',
            templateUrl: 'src/views/mapView.html'
        })
        .state('home.analysis', {
            url: '/analysis',
            controller: 'analysisController',
            templateUrl: 'src/views/analysisView.html'
        })
        .state('home.mypromotions', {
            url: '/my-promotions',
            controller: 'vendorsPromotionsController',
            templateUrl: 'src/views/vendorsPromotionsView.html'
        })
})
.controller('appController', function ($scope) {
    $scope.test = "test";
})
.filter('reverse', function() {
    return function (items) {
        if (items != undefined)
        return items.slice().reverse();
    };
})
.config(function ($httpProvider) {
    var interceptor = function (userService, $q, $location, $state)
    {
        return {
            request: function (config) {
                var currentUser = userService.GetCurrentUser();
                if (currentUser != null) {
                    config.headers['Authorization'] = 'Bearer ' + currentUser.access_token;
                }
                return config;
            },
            responseError : function(rejection)
            {
                if (rejection.status === 401) {
                    $location.path('/login')
                    return $q.reject(rejection);
                }
                if (rejection.status === 403) {
                    $location.path('/unauthorized');
                    return $q.reject(rejection);
                }
                return $q.reject(rejection);
            }
        }
    }
    var params = ['userService', '$q', '$location'];
    interceptor.$inject = params;
    $httpProvider.interceptors.push(interceptor);
});


