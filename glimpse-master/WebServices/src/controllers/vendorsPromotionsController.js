'use strict';

app.controller('vendorsPromotionsController', ['$scope', 'dataService', '$state', '$uibModal', function ($scope, dataService, $state, $uibModal) {
    $scope.editOn = false;
    $scope.removeOn = false;
    $scope.maxPage = 0;
    $scope.currentPage = 1;
    dataService.GetAuthorizeData().then(function (data) {
        console.log("Authorized");
    }, function (error) {
        console.log("No longer logged in");
        alert("You have been logged out due to session timeout");
    })

    var getVendorsPromotion = function () {
        var promotionsquery = dataService.getAllPromotionFromSpecificVendor(localStorage.id).query();
        promotionsquery.$promise.then(function (data) {
            $scope.mypromotions = data;
            $scope.maxPage = Math.ceil(data.length / 7);
        }, function (error) {
            console.log("Error: Could not load promotions");
        })
    }
    $scope.getNumber = function (num) {
        return new Array(num);
    }
    $scope.setCurrentPage = function (currentPage) {
        $scope.currentPage = currentPage;
    }
    $scope.setCurrentPageToNext = function () {
        if ($scope.currentPage + 1 <= $scope.maxPage)
            $scope.currentPage++;
    }
    $scope.setCurrentPageToPrev = function () {
        if ($scope.currentPage - 1 > 0)
            $scope.currentPage--;
    }
    getVendorsPromotion();
    $scope.showCreatePromotion = function () {
        $uibModal.open({
            templateUrl: '/src/views/createPromotion.html',
            controller: 'modalController',
            size: 'lg',
            scope: $scope,
            resolve: {
                promotionDetails: {},
                edit: false
            }
        }).result.then(function (result) {
            getVendorsPromotion();
        }, function () {
            console.log("Modal dismissed");
        });
    }


    $scope.editPromotion = function (promotion) {
        $uibModal.open({
            templateUrl: '/src/views/createPromotion.html',
            controller: 'modalController',
            size: 'lg',
            scope: $scope,
            resolve: {
                promotionDetails: promotion,
                edit: true
            }
        }).result.then(function (updatedPromotionData) {
            $scope.mypromotions.forEach(function (element, index) {
                if (element.PromotionId === promotion.PromotionId) {
                    $scope.mypromotions[index] = updatedPromotionData;
                }
            });
        }, function () {
            console.log("Modal dismissed");
        });
    }

    $scope.turnEditOn = function () {
        $scope.editOn = true;
        $scope.removeOn = false;
    }

    $scope.turnRemoveOn = function () {
        $scope.editOn = false;
        $scope.removeOn = true;
    }

    $scope.turnOff = function () {
        $scope.editOn = false;
        $scope.removeOn = false;
    }

    $scope.removePromotion = function (promotion, index) {
        $uibModal.open({
            templateUrl: '/src/views/deletePromotionConfirmation.html',
            controller: 'deletePromotionModalController',
            size: 'lg',
            scope: $scope
        }).result.then(function (result) {
            dataService.deletePromotion().delete({
                promotion: promotion.PromotionId
            }).$promise.then(function () {
                $scope.mypromotions.splice(index, 1);
            });
        }, function () {
            console.log("Modal dismissed");
        });
    }

    $scope.isPromotionExpired = function (promotionEndDate) {
        var currentDate = new Date();
        var promotionEndDate = new Date(promotionEndDate);
        promotionEndDate.setHours(0, 0, 0, 0);
        currentDate.setHours(0, 0, 0, 0);
        return currentDate.getTime() > promotionEndDate.getTime();
    }

    $scope.editPromotionDate = function (promotion) {
        $uibModal.open({
            templateUrl: '/src/views/changePromotionDate.html',
            controller: 'changeDateModalController',
            size: 'lg',
            scope: $scope,
            resolve: {
                promotionDetails: promotion
            }
        }).result.then(function (result) {
            console.log(result);
            console.log($scope.promotions);
            promotion["PromotionStartDate"] = result.startDate;
            promotion["PromotionEndDate"] = result.endDate;

            dataService.updatePromotion().update({
                promotion: promotion.PromotionId
            }, promotion).$promise.then(function (user) {
                $scope.mypromotions.forEach(function (element, index) {
                    if (element.PromotionId === promotion.PromotionId) {
                        $scope.mypromotions[index].PromotionStartDate = result.startDate;
                        $scope.mypromotions[index].PromotionEndDate = result.endDate;
                    }
                });
            });
        }, function () {
            console.log("Modal dismissed");
        });
    }

}]);

app.controller('modalController', function ($scope, $uibModalInstance, Upload, $timeout, dataService, $http, promotionDetails, $q, edit) {
    $scope.edit = edit;
    $scope.promotionTitle = promotionDetails.Title || '';
    if (promotionDetails.Category == 0) {
        $scope.category = 0;
    } else {
        $scope.category = promotionDetails.Category || '';
    }
    $scope.description = promotionDetails.Description || '';
    $scope.promotionDescription = promotionDetails.Description || '';
    $scope.startDay = promotionDetails.PromotionStartDate || undefined;
    $scope.endDay = promotionDetails.PromotionEndDate || undefined;
    $scope.sdt = new Date($scope.startDay);
    $scope.edt = new Date($scope.endDay);
    $scope.showDateWarning = false;
    $scope.showTitleWarning = false;
    $scope.showDescriptionWarning = false;
    $scope.showCategorynWarning = false;
    $scope.showImageWarning = false;
    $scope.isResetEnable = false;
    $scope.isSilderFilterEnable = false;
    $scope.isAddImageBtn = true;
    $scope.previewImage = promotionDetails.PromotionImageURL ? "https://glimpseimages.blob.core.windows.net/imagestorage/" + promotionDetails.PromotionImageURL : '';
    var imageUrl = promotionDetails.PromotionImageURL ? "https://glimpseimages.blob.core.windows.net/imagestorage/" + promotionDetails.PromotionImageURL : '';

    if (imageUrl) {
        getBase64FromImageUrl(imageUrl).then(function (base64Image) {
            $scope.removeImage();
            resetSliderFilter();
            //$scope.isSilderFilterEnable = true;
            $scope.previewImage = base64Image;
            $('#previewImage').remove();
            if (!$('#previewImage').length) {
                var orriginalImag = $('#originalImage').clone();
                $(orriginalImag).removeClass('ng-hide');
                $(orriginalImag).removeClass('hide').attr('id', 'previewImage');
                $(orriginalImag).attr('src', base64Image);
                $('#originalImage').parent().append(orriginalImag);
            }
        });
    }
    $scope.imageNotEmpty = false;

    var slides = $scope.slides = [];//promotionDetails["PromotionImages"].length ? promotionDetails["PromotionImages"] : [];
    var currIndex = 0;

    function getImageData() {

        var defer = $q.defer();
        var imageData = null;
        if (typeof $scope.previewImage === 'object') {
            Upload.base64DataUrl($scope.previewImage).then(function (base64) {
                imageData = base64;
                defer.resolve(imageData);
            }).catch(function () {
                defer.resolve(imageData);
            });
        } else {
            imageData = $scope.previewImage;
            defer.resolve(imageData);
        }
        return defer.promise;
    }

    function getBase64FromImageUrl(url) {
        return $q(function (resolve, reject) {
            var img = new Image();

            img.setAttribute('crossOrigin', 'anonymous');

            img.onload = function () {
                var canvas = document.createElement("canvas");
                canvas.width = this.width;
                canvas.height = this.height;

                var ctx = canvas.getContext("2d");
                ctx.drawImage(this, 0, 0);

                var dataURL = canvas.toDataURL("image/jpeg");
                resolve(dataURL);

                var url = dataURL.replace(/^data:image\/(png|jpg|jpeg);base64,/, "");
            };

            img.src = url;
        });
    }

    function getSliderImagesList() {
        var sliderImages = $scope.slides.length ? $scope.slides : [];
        var images = [];

        sliderImages.forEach(function (element, index) {
            images.push(element.split(',')[1])
        });
        return images
    }

    $scope.ok = function () {
        if ($scope.sdt > $scope.edt)
            $scope.showDateWarning = true;
        else if ($scope.promotionTitle.length == 0) {
            $scope.showDateWarning = false;
            $scope.showTitleWarning = true;
        }
        else if ($scope.promotionDescription.length == 0) {
            $scope.showTitleWarning = false;
            $scope.showDescriptionWarning = true;
        }
        else if ($scope.category == undefined || $scope.category === "") {
            $scope.showDescriptionWarning = false;
            $scope.showTitleWarning = false;
            $scope.showCategorynWarning = true;
        }
        else if ($scope.previewImage == null || $scope.previewImage.length == 0) {
            $scope.showCategorynWarning = false;
            $scope.showDescriptionWarning = false;
            $scope.showTitleWarning = false;
            $scope.showImageWarning = true;
        }

        else {
            var isEditMode = !(angular.equals({}, promotionDetails));
            var sdt = $scope.sdt;
            var edt = $scope.edt;
            var promotionTitleForPicture = $scope.promotionTitle.split(' ').join('');
            var PromotionImages = [];
            sdt.setHours(0, 0, 0, 0);
            edt.setHours(0, 0, 0, 0);
            var promotionData = {
                RequestFromWeb : true,
                title: $scope.promotionTitle,
                description: $scope.promotionDescription,
                category: $scope.category,
                promotionStartDate: sdt,
                promotionEndDate: edt,
                promotionImages: PromotionImages,
                promotionImageURL: localStorage.id + "/" + promotionTitleForPicture + "/" + "cover"
            }

            if (isEditMode) {
                promotionData["vendorId"] = promotionDetails.VendorId;
            } else {
                promotionData["vendorId"] = localStorage.id;
            }

            if (!!$scope.previewImage) {
                getImageData().then(function (imageBased64) {
                    promotionData["promotionImage"] = imageBased64.split(",")[1];
                    if (isEditMode) {
                        onEditClick(promotionDetails.PromotionId, promotionData);
                    } else {
                        onSaveClick();
                    }
                });

            } else {
                promotionData["promotionImage"] = null;
                if (isEditMode) {
                    onEditClick(promotionDetails.PromotionId, promotionData);
                } else {
                    onSaveClick();
                }
            }

            function onSaveClick() {
                var promotionImages = getSliderImagesList();
                var promotionsImagesURL = [];


                var promotionImageInfo = [];

                promotionImages.forEach(function (imageBase64, index) {
                    var extraImage = {
                        //PromotionId: promotionData.vendorId,
                        //PromotionImageId: promotionData.vendorId + index,
                        Image : imageBase64,
                        ImageURL : promotionData.vendorId + "/" + promotionData.title.split(' ').join('') + "/" + "image" + index
                    }
                    promotionImageInfo.push(extraImage);
                });

                promotionData['promotionImages'] = promotionImageInfo;
                console.log(promotionData);
                dataService.getPromotions().save(promotionData, function (resp, headers) {
                    $uibModalInstance.close(promotionData);
                },
                function (err) {
                    if (err.status == 500)
                        $uibModalInstance.close(promotionData);
                });
            }
            function onEditClick(promotionId, promotion) {
                var promotionData = {};
                promotionData["Category"] = promotion.category || '';
                if (promotionData["Category"] == '')
                    promotionData["Category"] = 0;
                promotionData["Description"] = promotion.description || '';
                promotionData["PromotionEndDate"] = promotion["promotionEndDate"];
                promotionData["PromotionId"] = promotionId;
                promotionData["PromotionStartDate"] = promotion["promotionStartDate"];
                promotionData["PromotionImageURL"] = promotion.promotionImageURL || '';
                promotionData["PromotionImage"] = promotion.promotionImage;
                promotionData["Title"] = promotion["title"];
                promotionData["Vendor"] = promotion["vendor"] || null;
                promotionData["VendorId"] = promotion["vendorId"];
                promotionData["PromotionEndDate"].setHours(0, 0, 0, 0);
                promotionData["PromotionStartDate"].setHours(0, 0, 0, 0);
                dataService.updatePromotion().update({
                    promotion: promotionId
                }, promotionData).$promise.then(function () {
                    $uibModalInstance.close(promotionData);
                }).catch(function (err) {
                    console.log(err);
                    $uibModalInstance.close({});
                });
            }
        }
    };

    $scope.arrayBufferToBase64 = function (buffer) {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };

    $scope.closeWarning = function (warning) {
        switch (warning) {
            case 0:
                $scope.showDateWarning = false;
                break;
            case 1:
                $scope.showTitleWarning = false;
                break;
            case 2:
                $scope.showDescriptionWarning = false;
                break;
            case 3:
                $scope.showCategorynWarning = false;
                break;
            case 4:
                $scope.showImageWarning = false;
                break;
            default:
                break;
        }
    }

    $scope.croppedDataUrl = '';
    $scope.isCropImageEnable = false;
    $scope.saveCrop = false;
    var imageFile = '';

    $scope.$watch('picFile', function () {
        if (!!$scope.picFile) {
            imageFile = $scope.picFile;
            $scope.removeImage();
            $scope.isAddImageBtn = true;
            resetSliderFilter();
            //$scope.isSilderFilterEnable = true;
            $scope.previewImage = imageFile;
            Upload.base64DataUrl(imageFile).then(function (urls) {
                $('#previewImage').remove();
                if (!$('#previewImage').length) {
                    var orriginalImag = $('#originalImage').clone();
                    $(orriginalImag).removeClass('ng-hide');
                    $(orriginalImag).removeClass('hide').attr('id', 'previewImage');
                    $(orriginalImag).attr('src', urls);
                    $('#originalImage').parent().append(orriginalImag);
                }
            });
        }
    });



    function addSlide(image) {
        var newWidth = 600 + slides.length + 1;
        slides.push(image);
    };

    $scope.direction = 'left';
    $scope.currentIndex = 0;

    $scope.setCurrentSlideIndex = function (index) {
        $scope.direction = (index > $scope.currentIndex) ? 'left' : 'right';
        $scope.currentIndex = index;
    };

    $scope.isCurrentSlideIndex = function (index) {
        return $scope.currentIndex === index;
    };

    $scope.prevSlide = function () {
        $scope.direction = 'left';
        $scope.currentIndex = ($scope.currentIndex < $scope.slides.length - 1) ? ++$scope.currentIndex : 0;
    };

    $scope.nextSlide = function () {
        $scope.direction = 'right';
        $scope.currentIndex = ($scope.currentIndex > 0) ? --$scope.currentIndex : $scope.slides.length - 1;
    };

    $scope.cropImage = function () {
        $scope.isCropImageEnable = true;
        $scope.saveCrop = true;
        if (!!$scope.cropper) {
            $scope.cropper.destroy();
        }
        var image = document.getElementById('previewImage');
        $scope.cropper = new Cropper(image, options);
    }

    $scope.doneCrop = function () {
        $scope.isCropImageEnable = false;
        $scope.saveCrop = false;
        $scope.previewImage = $scope.cropper.getCroppedCanvas().toDataURL('image/jpeg');
        var image = document.getElementById('previewImage');

        if ($(image).is('canvas')) {

            var orriginalImag = $('#originalImage').clone();
            $(orriginalImag).removeClass('hide').attr('id', 'previewImage');
            $(orriginalImag).attr('src', $scope.previewImage);

            $('#originalImage').parent().append(orriginalImag);
            $(image).remove();
        } else {
            $('#previewImage').attr('src', $scope.previewImage);
        }

        $('#previewImage').removeClass('cropper-hidden');
        $('.cropper-container').remove();
    }

    $scope.cancelCrop = function () {
        $scope.isCropImageEnable = false;
        $scope.saveCrop = false;
        if (!!$('#previewImage')) {
            $('#previewImage').removeClass('cropper-hidden');
            $('.cropper-container').remove();
        }

        if (!!$scope.cropper) {
            $scope.cropper.destroy();
        }
    }

    $scope.selectedFilter = 'Apply Filters';
    $scope.applyFilter = function (filterType) {
        $scope.selectedFilter = filterType || 'Apply Filters';
        if (filterType === "Custom Filter") {
            resetSliderFilter();
            $scope.isSilderFilterEnable = true;
            return;
        } else {
            $scope.isSilderFilterEnable = false;
        }


        Caman("#previewImage", function () {
            if ($scope.isResetEnable) {
                this.reset(function () { });
            }

            switch (filterType) {
                case 'Vintage':
                    this.greyscale();
                    this.contrast(5);
                    this.noise(3);
                    this.sepia(100);
                    this.channels({
                        red: 8,
                        blue: 2,
                        green: 4
                    });
                    this.gamma(0.87);
                    break;
                case 'Lomo':
                    this.brightness(15);
                    this.exposure(15);
                    this.curves("rgb", [0, 0], [200, 0], [155, 255], [255, 255]);
                    this.saturation(-20);
                    this.gamma(1.8);
                    break;
                case 'Clarity':
                    this.vibrance(20);
                    this.curves("rgb", [5, 0], [130, 150], [190, 220], [250, 255]);
                    this.sharpen(15);
                    this.vignette("45%", 20);
                    break;
                case 'SinCity':
                    this.contrast(100);
                    this.brightness(15);
                    this.exposure(10);
                    this.posterize(80);
                    this.clip(30);
                    this.greyscale()
                    break;
                case 'Sunrise':
                    this.exposure(3.5);
                    this.saturation(-5);
                    this.vibrance(50);
                    this.sepia(60);
                    this.colorize("#e87b22", 10);
                    this.channels({
                        red: 8,
                        blue: 8
                    });
                    this.contrast(5);
                    this.gamma(1.2);
                    break;
                case 'CrossProcess':
                    this.exposure(5);
                    this.colorize("#e87b22", 4);
                    this.sepia(20);
                    this.channels({
                        blue: 8,
                        red: 3
                    });
                    this.curves("b", [0, 0], [100, 150], [180, 180], [255, 255]);
                    this.contrast(15);
                    this.vibrance(75);
                    this.gamma(1.6);
                    break;
                case 'OrangePeel':
                    this.curves("rgb", [0, 0], [100, 50], [140, 200], [255, 255]);
                    this.vibrance(-30);
                    this.saturation(-30);
                    this.colorize("#ff9000", 30);
                    this.contrast(-5);
                    this.gamma(1.4);
                    break;
                case 'Grungy':
                    this.gamma(1.5);
                    this.clip(25);
                    this.saturation(-60);
                    this.contrast(5);
                    this.noise(5);
                    this.vignette("50%", 30);
                    break;
                case 'Jarques':
                    this.saturation(-35);
                    this.curves("b", [20, 0], [90, 120], [186, 144], [255, 230]);
                    this.curves("r", [0, 0], [144, 90], [138, 120], [255, 255]);
                    this.curves("g", [10, 0], [115, 105], [148, 100], [255, 248]);
                    this.curves("rgb", [0, 0], [120, 100], [128, 140], [255, 255]);
                    this.sharpen(20)
                    break;
                case 'Pinhole':
                    this.greyscale();
                    this.sepia(10);
                    this.exposure(10);
                    this.contrast(15);
                    this.vignette("60%", 35);
                    break;
                case 'OldBoot':
                    this.saturation(-20);
                    this.vibrance(-50);
                    this.gamma(1.1);
                    this.sepia(30);
                    this.channels({
                        red: -10,
                        blue: 5
                    });
                    this.curves("rgb", [0, 0], [80, 50], [128, 230], [255, 255]);
                    this.vignette("60%", 30);
                    break;
                case 'GlowingSun':
                    this.brightness(10);
                    this.newLayer(function () {
                        this.setBlendingMode("multiply");
                        this.opacity(80);
                        this.copyParent();
                        this.filter.gamma(0.8);
                        this.filter.contrast(50);
                        this.filter.exposure(10)
                    });
                    this.newLayer(function () {
                        this.setBlendingMode("softLight");
                        this.opacity(80);
                        this.fillColor("#f49600")
                    });
                    this.exposure(20);
                    this.gamma(0.8);
                    break;
                case 'HazyDays':
                    this.gamma(1.2);
                    this.newLayer(function () {
                        this.setBlendingMode("overlay");
                        this.opacity(60);
                        this.copyParent();
                        this.filter.channels({
                            red: 5
                        });
                        this.filter.stackBlur(15)
                    });
                    this.newLayer(function () {
                        this.setBlendingMode("addition");
                        this.opacity(40);
                        this.fillColor("#6899ba")
                    });
                    this.newLayer(function () {
                        this.setBlendingMode("multiply");
                        this.opacity(35);
                        this.copyParent();
                        this.filter.brightness(40);
                        this.filter.vibrance(40);
                        this.filter.exposure(30);
                        this.filter.contrast(15);
                        this.filter.curves("r", [0, 40], [128, 128], [128, 128], [255, 215]);
                        this.filter.curves("g", [0, 40], [128, 128], [128, 128], [255, 215]);
                        this.filter.curves("b", [0, 40], [128, 128], [128, 128], [255, 215]);
                        this.filter.stackBlur(5)
                    });
                    this.curves("r", [20, 0], [128, 158], [128, 128], [235, 255]);
                    this.curves("g", [20, 0], [128, 128], [128, 128], [235, 255]);
                    this.curves("b", [20, 0], [128, 108], [128, 128], [235, 255]);
                    break;
                case 'Nostalgia':
                    this.saturation(20);
                    this.gamma(1.4);
                    this.greyscale();
                    this.contrast(5);
                    this.sepia(100);
                    this.channels({
                        red: 8,
                        blue: 2,
                        green: 4
                    });
                    this.gamma(0.8);
                    this.contrast(5);
                    this.exposure(10);
                    this.newLayer(function () {
                        this.setBlendingMode("overlay");
                        this.copyParent();
                        this.opacity(55);
                        this.filter.stackBlur(10)
                    });
                    this.vignette("50%", 30)
                    break;
                case 'Hemingway':
                    this.greyscale();
                    this.contrast(10);
                    this.gamma(0.9);
                    this.newLayer(function () {
                        this.setBlendingMode("multiply");
                        this.opacity(40);
                        this.copyParent();
                        this.filter.exposure(15);
                        this.filter.contrast(15);
                        this.filter.channels({
                            green: 10,
                            red: 5
                        })
                    });
                    this.sepia(30);
                    this.curves("rgb", [0, 10], [120, 90], [180, 200], [235, 255]);
                    this.channels({
                        red: 5,
                        green: -2
                    });
                    this.exposure(15);
                    break;
                case 'Concentrate':
                    this.sharpen(40);
                    this.saturation(-50);
                    this.channels({
                        red: 3
                    });
                    this.newLayer(function () {
                        this.setBlendingMode("multiply");
                        this.opacity(80);
                        this.copyParent();
                        this.filter.sharpen(5);
                        this.filter.contrast(50);
                        this.filter.exposure(10);
                        this.filter.channels({
                            blue: 5
                        });
                    });
                    this.brightness(10)
                    break;
                case 'HerMajesty':
                    this.brightness(40);
                    this.colorize("#ea1c5d", 10);
                    this.curves("b", [0, 10], [128, 180], [190, 190], [255, 255]);
                    this.newLayer(function () {
                        this.setBlendingMode("overlay");
                        this.opacity(50);
                        this.copyParent();
                        this.filter.gamma(0.7);
                        this.newLayer(function () {
                            this.setBlendingMode("normal");
                            this.opacity(60);
                            this.fillColor("#ea1c5d")
                        })
                    });
                    this.newLayer(function () {
                        this.setBlendingMode("multiply");
                        this.opacity(60);
                        this.copyParent();
                        this.filter.saturation(50);
                        this.filter.hue(90);
                        this.filter.contrast(10)
                    });
                    this.gamma(1.4);
                    this.vibrance(-30);
                    this.newLayer(function () {
                        this.opacity(10);
                        this.fillColor("#e5f0ff")
                    });
                    break;
                default:
                    break;
            }
            this.render(function () {
                $scope.$apply(function () {
                    $scope.isResetEnable = true;
                })
            });
            $('#previewImage').css({
                height: '100%',
                width: 'auto'
            })
        });
    }

    $scope.applySilderFilter = function () {
        Caman("#previewImage", function () {

            if ($scope.isResetEnable) {
                this.reset(function () { });
            }
            this.brightness($scope.brightness.value)
                .contrast($scope.contrast.value)
                .sepia($scope.sepia.value)
                .saturation($scope.saturation.value)
                .noise($scope.noise.value)
                .clip($scope.clip.value)
                .exposure($scope.exposure.value)
                .hue($scope.hue.value)
                .vibrance($scope.vibrance.value)
                .sharpen($scope.sharpen.value)
                .stackBlur($scope.stackblur.value)
                .render(function () {
                    $scope.$apply(function () {
                        $scope.isResetEnable = true;
                    });
                });

            $('#previewImage').css({
                height: '100%',
                width: 'auto'
            });
        });
    }

    function resetSliderFilter() {
        $scope.brightness.value = 0;
        $scope.contrast.value = 0;
        $scope.vibrance.value = 0;
        $scope.saturation.value = 0;
        $scope.hue.value = 0;
        $scope.exposure.value = 0;
        $scope.sepia.value = 0;
        $scope.clip.value = 0;
        $scope.noise.value = 0;
        $scope.stackblur.value = 0;
        $scope.sharpen.value = 0;
    }

    $scope.brightness = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.contrast = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.vibrance = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.saturation = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.hue = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.exposure = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.gamma = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.sepia = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.clip = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.noise = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.stackblur = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }
    $scope.sharpen = {
        value: 0,
        options: {
            floor: 0,
            ceil: 100,
            onEnd: function () {
                $scope.applySilderFilter();
            }
        }
    }


    $scope.showButtons = function () {
        $scope.imageNotEmpty = true;
    }

    var options = {
        aspectRatio: 16 / 9
    };

    /*
        Save Uploaded Image into slider
     */
    $scope.saveImage = function () {
        getImageData().then(function (imageBased64) {
            addSlide(imageBased64);
            $scope.isAddImageBtn = false;
        });
    }

    $scope.removeImage = function () {
        $scope.imageNotEmpty = false;
        $scope.picFile = null;
        $scope.isSilderFilterEnable = false;
        $scope.croppedDataUrl = null;
        $scope.isCropImageEnable = false;
        $scope.previewImage = "";
        var image = document.getElementById('previewImage');
        if ($(image).is('canvas')) {
            $(image).remove();

        } else {
            $scope.previewImage = '';
            $('#previewImage').attr('src', '' );
        }
        $('#originalImage').attr('src', '');
        $scope.selectedFilter = 'Apply Filters';
    }
    $scope.resetFilter = function () {
        $scope.isResetEnable = false;
        $scope.isSilderFilterEnable = false;
        $scope.selectedFilter = 'Apply Filters';
        Caman("#previewImage", function () {
            this.reset();
        });
    }
    $scope.saveFilter = function () {
        $scope.isResetEnable = false;
        $scope.isSilderFilterEnable = false;
        Caman("#previewImage", function () {
            var imageBase64data = this.toBase64('jpeg');
            $scope.$apply(function () {
                $scope.previewImage = imageBase64data;
                $('#previewImage').remove();
                if (!$('#previewImage').length) {
                    var orriginalImag = $('#originalImage').clone();
                    $(orriginalImag).removeClass('ng-hide');
                    $(orriginalImag).removeClass('hide').attr('id', 'previewImage');
                    $(orriginalImag).attr('src', imageBase64data);
                    $('#originalImage').parent().append(orriginalImag);
                }
            });
        });
    }
    $scope.today = function () {
        $scope.sdt = new Date();
        $scope.edt = new Date();
    };
    if (!($scope.startDay || $scope.endDay)) {
        $scope.today();
    }
    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };

    $scope.startDateOptions = {
        format: 'dd-MMMM-yyyy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.endDateOptions = {
        format: 'dd-MMMM-yyyy',
        maxDate: new Date(2020, 5, 22),
        minDate: $scope.sdt,
        startingDay: 1
    };

    $scope.openStartDate = function () {
        $scope.startDate.opened = true;
    };

    $scope.openEndDate = function () {
        $scope.endDate.opened = true;
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];

    $scope.startDate = {
        opened: false
    };

    $scope.endDate = {
        opened: false
    };

    function getDayClass(data) {
        var date = data.date,
          mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }

}).animation('.slide-animation', function () {
    return {
        beforeAddClass: function (element, className, done) {
            var scope = element.scope();

            if (className == 'ng-hide') {
                var finishPoint = element.parent().width();
                if (scope.direction !== 'right') {
                    finishPoint = -finishPoint;
                }
                TweenMax.to(element, 0.5, { left: finishPoint, onComplete: done });
            }
            else {
                done();
            }
        },
        removeClass: function (element, className, done) {
            var scope = element.scope();

            if (className == 'ng-hide') {
                element.removeClass('ng-hide');

                var startPoint = element.parent().width();
                if (scope.direction === 'right') {
                    startPoint = -startPoint;
                }

                TweenMax.fromTo(element, 0.5, { left: startPoint }, { left: 0, onComplete: done });
            }
            else {
                done();
            }
        }
    };
});
app.controller('changeDateModalController', function ($scope, $uibModalInstance, promotionDetails) {

    $scope.sdt = new Date(promotionDetails.PromotionStartDate);
    console.log($scope.sdt);
    $scope.sdt.setHours($scope.sdt.getHours() + 5);
    $scope.edt = new Date(promotionDetails.PromotionEndDate);
    $scope.edt.setHours($scope.edt.getHours() + 5);
    $scope.showDateWarning = false;

    $scope.done = function () {

        if ($scope.sdt > $scope.edt) {
            $scope.showDateWarning = true;

        } else {
            $uibModalInstance.close({ startDate: $scope.sdt, endDate: $scope.edt });
        }

    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }

    $scope.closeWarning = function (warning) {
        switch (warning) {
            case 0:
                $scope.showDateWarning = false;
                break;
            case 1:
                $scope.showTitleWarning = false;
                break;
            case 2:
                $scope.showDescriptionWarning = false;
                break;
            case 3:
                $scope.showCategorynWarning = false;
                break;
            case 4:
                $scope.showImageWarning = false;
                break;
            default:
                break;
        }
    }

    $scope.inlineOptions = {
        customClass: getDayClass,
        minDate: new Date(),
        showWeeks: true
    };

    $scope.startDateOptions = {
        format: 'dd-MMMM-yyyy',
        maxDate: new Date(2020, 5, 22),
        minDate: new Date(),
        startingDay: 1
    };

    $scope.endDateOptions = {
        format: 'dd-MMMM-yyyy',
        maxDate: new Date(2020, 5, 22),
        minDate: $scope.sdt,
        startingDay: 1
    };

    $scope.openStartDate = function () {
        $scope.startDate.opened = true;
    };

    $scope.openEndDate = function () {
        $scope.endDate.opened = true;
    };

    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
    $scope.format = $scope.formats[0];

    $scope.startDate = {
        opened: false
    };

    $scope.endDate = {
        opened: false
    };

    function getDayClass(data) {
        var date = data.date,
			mode = data.mode;
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    }

});

app.controller('deletePromotionModalController', function ($scope, $uibModalInstance) {

    $scope.confirm = function () {
        $uibModalInstance.close('Confirmed');
    }

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    }
});