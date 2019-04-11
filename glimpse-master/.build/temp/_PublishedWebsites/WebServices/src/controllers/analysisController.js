'use strict';
app.controller('analysisController', ['$scope', 'dataService', function ($scope, dataService) {

    $scope.data = [];
    $scope.dataHours = [];
    $scope.dataDays = [];
    $scope.totalClicked = [];
    $scope.vendorPromotionsClicked = [];
    $scope.series = [];
    $scope.seriesTitle = [];
    $scope.noPromotionClicked = false;
    var promotionsquery = dataService.getAllPromotionFromSpecificVendor(localStorage.id).query();
    promotionsquery.$promise.then(function (data) {
        $scope.promotions = data;
        getPromotionClicks();
    }, function (error) {
        console.log("Error: Could not load promotions");
    })

    var getPromotionClicks = function () {
        var promotionClicksquery = dataService.getPromotionClicks().query();
        promotionClicksquery.$promise.then(function (data) {
            $scope.promotionClicks = data;
            getVendorPromotionsClicked();
        }, function (error) {
            console.log("Error: Could not load promotions");
        })
    }
    
    var getVendorPromotionsClicked = function () {
        angular.forEach($scope.promotions, function (element, index) {
            angular.forEach($scope.promotionClicks, function(element1, index1){
                if (element.PromotionId == element1.PromotionId) {
                    if (element.clicks == undefined)
                        element.clicks = 0;
                    else {
                        element.clicks++;
                    }
                    element1.title = element.Title;
                    $scope.vendorPromotionsClicked.push(element1);
                    var newDate = new Date(element1.Time);
                }
            })
        });
        if ($scope.vendorPromotionsClicked.length == 0) {
            $scope.noPromotionClicked = true;
        }
        insertData();
    }
    
    var insertData = function () {
        angular.forEach($scope.vendorPromotionsClicked, function (element, index) {
            if ($scope.series.indexOf(element.PromotionId) == -1) {
                $scope.series.push(element.PromotionId);
                $scope.seriesTitle.push(element.title);
            }
        })
        initializeData();
        angular.forEach($scope.series, function (serie, indexSerie) {
            angular.forEach($scope.vendorPromotionsClicked, function (elementClicked, indexClicked) {
                if (serie == elementClicked.PromotionId) {
                    var newDate = new Date(elementClicked.Time);
                    var date = newDate.getDate();
                    var time = newDate.getHours() + 5;
                    var day = newDate.getDay() - 1;
                    $scope.dataHours[indexSerie][time]++;
                    $scope.dataDays[indexSerie][day]++;
                    switch(date) {
                        case $scope.labels[0]:
                            $scope.data[indexSerie][0]++;
                            break;
                        case $scope.labels[1]:
                            $scope.data[indexSerie][1]++;
                            break;
                        case $scope.labels[2]:
                            $scope.data[indexSerie][2]++;
                            break;
                        case $scope.labels[3]:
                            $scope.data[indexSerie][3]++;
                            break;
                        case $scope.labels[4]:
                            $scope.data[indexSerie][4]++;
                            break;
                        case $scope.labels[5]:
                            $scope.data[indexSerie][5]++;
                            break;
                        case $scope.labels[6]:
                            $scope.data[indexSerie][6]++;
                            break;
                        default:
                            break;
                    }

                }
            })
        })

        angular.forEach($scope.data, function (element, index) {
            angular.forEach(element, function (element1, index1) {
                $scope.totalClicked[index] += element1;
            })
        })
    }

    var today = new Date();
    $scope.labels = [moment().subtract(6, 'days').format('MMM Do'), moment().subtract(5, 'days').format('MMM Do'), moment().subtract(4, 'days').format('MMM Do'), moment().subtract(3, 'days').format('MMM Do'), moment().subtract(2, 'days').format('MMM Do'), moment().subtract(1, 'days').format('MMM Do'), moment().format('MMM Do')];
    $scope.labelHours = ["00:00", "01:00", "02:00", "03:00", "04:00", "05:00", "06:00", "07:00", "08:00", "09:00", "10:00", "11:00", "12:00", "13:00", "14:00", "15:00", "16:00", "17:00", "18:00", "19:00", "20:00", "21:00", "22:00", "23:00"];
    $scope.labelDays = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

    var initializeData = function () {
        
        angular.forEach($scope.series, function (serie, indexSerie) {
            $scope.data[indexSerie] = [0, 0, 0, 0, 0, 0, 0];
            $scope.dataHours[indexSerie] = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            $scope.dataDays[indexSerie] = [0, 0, 0, 0, 0, 0, 0];
            $scope.totalClicked[indexSerie] = 0;
        });
    }

    $scope.onClick = function (points, evt) {
        console.log(points, evt);
    };
    moment().format('MMMM Do YYYY, h:mm:ss a');
    moment().format('MMMM Do YYYY, h:mm:ss a')

    $scope.datasetOverride = [{ yAxisID: 'y-axis-1' }];
    $scope.chartLineDatesOptions = {
        scales: {
            yAxes: [
              {
                  id: 'y-axis-1',
                  scaleLabel: {
                      display: true,
                      labelString: 'Number of Views'
                  },
                  type: 'linear',
                  display: true,
                  position: 'left',
                  ticks: {
                      beginAtZero: true,
                      callback: function (value) { if (value % 1 === 0) { return value; } }
                  }
              },
            ]
        },
        legend: {display: true}
    };
    $scope.chartLineDaysOptions = {
        scales: {
            yAxes: [
              {
                  id: 'y-axis-1',
                  scaleLabel: {
                      display: true,
                      labelString: 'Number of Views'
                  },
                  type: 'linear',
                  display: true,
                  position: 'left',
                  ticks: {
                      beginAtZero: true,
                      callback: function (value) { if (value % 1 === 0) { return value; } }
                  }
              }
            ]
        },
        legend: { display: true }
    };

}]);
