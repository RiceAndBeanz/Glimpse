// Karma configuration
// Generated on Mon Jun 16 2014 15:04:49 GMT+1000 (AUS Eastern Standard Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
    'http://maps.googleapis.com/maps/api/js?sensor=false&language=en',
    "Scripts/angular.min.js",
    "Scripts/jquery-3.1.1.min.js",
    "Scripts/angular-mocks.js",
    "Scripts/angular-route.min.js",
    "Scripts/angular-ui-router.min.js",
    "Scripts/angular-animate.min.js",
    "Scripts/angular-touch.min.js",
    "Scripts/angular-resource.min.js",
    "Scripts/angular-block-ui.min.js",
    "Scripts/Chart.min.js",
    "Scripts/angular-chart.min.js",
    "Scripts/tether.min.js",
    "Scripts/bootstrap.min.js",
    "Scripts/ng-map.min.js",
    "Scripts/ui-cropper.js",
    "Scripts/angular-ui/ui-bootstrap-tpls.min.js",
    "Scripts/lodash.min.js",
    "Scripts/angular-local-storage.min.js",
    "Scripts/angular-simple-logger.min.js",
    "Scripts/angular-google-maps.min.js",
    "Scripts/ng-file-upload-shim.min.js",
    "Scripts/ng-file-upload.min.js",
    "Scripts/rzslider.min.js",
    "Scripts/TweenMax.min.js",
    "Scripts/cropper.min.js",
    "Scripts/moment.min.js",
    "app.js",
    "src/controllers/*.js",
    "src/services/*.js",
    "Scripts/angular-mocks.js",
    "TestScripts/*.js",
    ],


    // list of files to exclude
    exclude: [
             
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress', 'xml'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: false,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: true
  });
  
};
