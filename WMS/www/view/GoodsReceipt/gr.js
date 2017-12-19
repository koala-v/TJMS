appControllers.controller('GrListCtrl', [
    'ENV',
    '$scope',
    '$stateParams',
    '$state',
    '$cordovaKeyboard',
    'ApiService',
    function (
        ENV,
        $scope,
        $stateParams,
        $state,
        $cordovaKeyboard,
        ApiService) {
        $scope.Tjms1 = {};
        $scope.GrnNo = {};
        $scope.Imgr1s = {};
        $scope.refreshRcbp1 = function (BusinessPartyName) {
            if (is.not.undefined(BusinessPartyName) && is.not.empty(BusinessPartyName)) {
                var objUri = ApiService.Uri(true, '/api/tms/rcbp1');
                objUri.addSearch('BusinessPartyName', BusinessPartyName);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Rcbp1s = result.data.results;
                });
            }
        };
        $scope.refreshJobNos = function (Job) {
            if (is.not.undefined(Job) && is.not.empty(Job)) {
                var objUri = ApiService.Uri(true, '/api/tms/tjms1');
                objUri.addSearch('JobNo', Job);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.JobNos = result.data.results;
                });
            }
        };
        $scope.ShowTjms1 = function (Customer) {
            if (is.not.undefined(Customer) && is.not.empty(Customer)) {
                var objUri = ApiService.Uri(true, '/api/tms/tjms1');
                objUri.addSearch('CustomerCode', Customer);
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.Tjms1s = result.data.results;
                });
            }
            if (!ENV.fromWeb) {
                $cordovaKeyboard.close();
            }
        };
        $scope.GoToDetail = function (Tjms1) {
            if (Tjms1 !== null) {
                $state.go('grDetail', {
                    'TrxNo': Tjms1.TrxNo,
                }, {
                    reload: true
                });
            }
        };
        $scope.returnMain = function () {
            $state.go('index.main', {}, {
                reload: true
            });
        };
    }
]);

appControllers.controller('GrDetailCtrl', [
    'ENV',
    '$scope',
    '$stateParams',
    '$state',
    '$http',
    '$timeout',
    '$ionicHistory',
    '$ionicLoading',
    '$ionicPopup',
    '$ionicModal',
    '$cordovaKeyboard',
    '$cordovaToast',
    '$cordovaBarcodeScanner',
    'ionicDatePicker',
    'SqlService',
    'ApiService',
    'PopupService',
    function (
        ENV,
        $scope,
        $stateParams,
        $state,
        $http,
        $timeout,
        $ionicHistory,
        $ionicLoading,
        $ionicPopup,
        $ionicModal,
        $cordovaKeyboard,
        $cordovaToast,
        $cordovaBarcodeScanner,
        ionicDatePicker,
        SqlService,
        ApiService,
        PopupService) {
        var popup = null;
        var dataResults = new Array();
        var hmImgr2 = new HashMap();
        var hmImsn1 = new HashMap();
        $scope.Detail = {
            TrxNo: $stateParams.TrxNo,
            Tjms2: {
                CargoDescription: '',
                Vessel: '',
            },
            tjms5: {},
            tote1: {},
            Tjms2s: {

            },
        };
        $scope.returnList = function () {
            // if ($ionicHistory.backView()) {
            //     $ionicHistory.goBack();
            // } else {
            $state.go('grList', {}, {
                reload: false
            });
            // }
        };

        $scope.gotoUpdateTjms5 = function (LineItemNo) {
            $state.go('grUpdateTjms5', {
                'TrxNo': $scope.Detail.TrxNo,
                'LineItemNo': LineItemNo,
            }, {
                reload: false
            });
            // }
        };
        $scope.gotoTjms5 = function () {
            $state.go('grTjms5', {
                'TrxNo': $scope.Detail.TrxNo,
            }, {
                reload: false
            });
            // }
        };
        var UpdateTjms2 = function () {
            var Tjms2Filter = "TrxNo='" + $scope.Detail.Tjms2.TrxNo + "' and LineItemNo='" + $scope.Detail.Tjms2.LineItemNo + "' ";
            var objTjms2 = {
                DateCompleted: $scope.Detail.Tjms2.DateCompleted,
                OfficeInChargeName: $scope.Detail.Tjms2.OfficeInChargeName,
                ChargeBerthQty: $scope.Detail.Tjms2.ChargeBerthQty,
                ChargeLiftingQty: $scope.Detail.Tjms2.ChargeLiftingQty,
                ChargeOther: $scope.Detail.Tjms2.ChargeOther,
                SignalManQty: $scope.Detail.Tjms2.SignalManQty
            };
            SqlService.Update('Tjms2', objTjms2, Tjms2Filter).then(function (res) {});
        };
        $scope.OnDatePicker = function () {
            var ipObj1 = {
                callback: function (val) { //Mandatory
                    //  console.log('Return value from the datepicker popup is : ' + val, new Date(val));
                    $scope.Detail.Tjms2.DateCompleted = moment(new Date(val)).format('YYYY-MM-DD');

                },
                to: new Date(),
            };
            ionicDatePicker.openDatePicker(ipObj1);
        };

        $scope.DeleteLine = function (LineItemNo) {
            if (LineItemNo > 0 && $scope.Detail.TrxNo !== "") {
                var objUri = ApiService.Uri(true, '/api/tms/tjms5/delete');
                objUri.addSearch('TrxNo', $scope.Detail.TrxNo);
                objUri.addSearch('LineItemNo', LineItemNo);
                ApiService.Get(objUri, false).then(function success(result) {
                    getTjms5($scope.Detail.TrxNo);

                });
            }
        };
        $scope.showTote1 = function (EquipmentType, LineItemNo) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1');
                objUri.addSearch('EquipmentType', EquipmentType.EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.tote1 = result.data.results;
                    if ($scope.Detail.tote1.length > 0) {
                        $scope.Detail.tjms5[LineItemNo - 1].EquipmentTypeDescription = $scope.Detail.tote1[0].EquipmentTypeDescription;
                        $scope.Detail.tjms5[LineItemNo - 1].Volume = $scope.Detail.tote1[0].Volume;
                        $scope.Detail.tjms5[LineItemNo - 1].ChargeWeight = $scope.Detail.tote1[0].ChgWt;
                        if ($scope.Detail.tote1[0].EditFlag === 'Y') {
                            $scope.Detail.tjms5[LineItemNo - 1].disabled = false;
                        } else {
                            $scope.Detail.tjms5[LineItemNo - 1].disabled = true;
                        }
                    }

                });
            }
        };
        $scope.refreshEquipmentType = function (EquipmentType) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1/EquipmentType');
                objUri.addSearch('EquipmentType', EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.EquipmentTypes = result.data.results;
                });
            }
        };
        var updateTjms5 = function () {
            if ($scope.Detail.tjms5.length > 0) {
                for (var i = 0; i < $scope.Detail.tjms5.length; i++) {
                    var objUri = ApiService.Uri(true, '/api/tms/tjms5/update');
                    objUri.addSearch('TrxNo', $scope.Detail.tjms5[i].TrxNo);
                    objUri.addSearch('LineItemNo', $scope.Detail.tjms5[i].LineItemNo);
                    objUri.addSearch('EquipmentType', $scope.Detail.tjms5[i].EquipmentType);
                    objUri.addSearch('EquipmentTypeDescription', $scope.Detail.tjms5[i].EquipmentTypeDescription);
                    objUri.addSearch('ContainerNo', $scope.Detail.tjms5[i].ContainerNo);
                    objUri.addSearch('Volume', $scope.Detail.tjms5[i].Volume);
                    objUri.addSearch('ChargeWeight', $scope.Detail.tjms5[i].ChargeWeight);
                    objUri.addSearch('ChgWtRoundUp', $scope.Detail.tjms5[i].ChgWtRoundUp);
                    objUri.addSearch('VehicleNo', $scope.Detail.tjms5[i].VehicleNo);
                    ApiService.Get(objUri, true).then(function success(result) {});
                }
                // for (var i = 0; i < $scope.Detail.tjms5.length; i++) {
                //     var arrtjms5 = [];
                //     arrtjms5.push($scope.Detail.tjms5[i]);
                //     var jsonData = {
                //         "UpdateAllString": JSON.stringify(arrtjms5)
                //     };
                //     var objUri = ApiService.Uri(true, '/api/tms/tjms5/update');
                //     ApiService.Post(objUri, jsonData, true).then(function success(result) {});
                // }

            }
        };
        $scope.gotoConfirm = function () {
            UpdateTjms2();
            updateTjms5();
            $state.go('jobListingConfirm', {
                'TrxNo': $scope.Detail.TrxNo,

            }, {
                reload: true
            });
        };
        $scope.gotoPrint = function () {

            $state.go('grPrint', {
                'TrxNo': $scope.Detail.TrxNo,
                'Flag': 'N',
            }, {
                reload: true
            });
        };
        var getSignature = function (objTjms2) {
            var objUri = ApiService.Uri(true, '/api/tms/tjms2/attach');
            objUri.addSearch('Key', objTjms2.JobNo);
            objUri.addSearch('LineItemNo', objTjms2.LineItemNo);
            objUri.addSearch('TableName', 'Tjms1');
            ApiService.Get(objUri, true).then(function success(result) {
                if (is.not.undefined(result.data.results)) {
                    $scope.signature = result.data.results;
                    var Tjms2Filter = "TrxNo='" + $scope.Detail.Tjms2.TrxNo + "' and LineItemNo='" + $scope.Detail.Tjms2.LineItemNo + "' ";
                    var objTjms2 = {
                        TempBase64: $scope.signature
                    };
                    SqlService.Update('Tjms2', objTjms2, Tjms2Filter).then(function (res) {});
                }
            });
        };
        var getjobs = function (obj) {
            var jobs = {
                TrxNo: obj.TrxNo,
                LineItemNo: obj.LineItemNo,
                disabled: false,
                EquipmentType: obj.EquipmentType,
                EquipmentTypeDescription: obj.EquipmentTypeDescription,
                ContainerNo: obj.ContainerNo,
                CargoDescription: obj.CargoDescription,
                Volume: obj.Volume,
                ChargeWeight: obj.ChargeWeight,
                ChgWtRoundUp: obj.ChgWtRoundUp,
                VehicleNo: obj.VehicleNo,
                StartDateTime: checkDateTimeisEmpty(obj.StartDateTime),
                EndDateTime: checkDateTimeisEmpty(obj.EndDateTime)
            };
            return jobs;
        };
        var getTjms5 = function (TrxNo) {
            var objUri = ApiService.Uri(true, '/api/tms/tjms5');
            objUri.addSearch('TrxNo', TrxNo);
            ApiService.Get(objUri, true).then(function success(result) {
                var results = result.data.results;
                dataResults = new Array();
                if (results.length > 0) {
                    // $scope.Detail.tjms5 = results;
                    for (var i = 0; i < results.length; i++) {
                        var jobs = getjobs(results[i]);
                        $scope.refreshEquipmentType(results[i].EquipmentType);
                        dataResults = dataResults.concat(jobs);
                        $scope.Detail.tjms5 = dataResults;
                    }
                } else {

                    $scope.Detail.tjms5 = dataResults;
                }
            });
        };

        var GetTjms2s = function (TrxNo) {
            var objUri = ApiService.Uri(true, '/api/tms/tjms2');
            objUri.addSearch('TrxNo', TrxNo);
            ApiService.Get(objUri, true).then(function success(result) {
                $scope.Detail.Tjms2s = result.data.results;
                SqlService.Delete('Tjms2').then(function (res) {
                    for (var i = 0; i < $scope.Detail.Tjms2s.length; i++) {
                        var objImgr2 = $scope.Detail.Tjms2s[i];
                        SqlService.Insert('Tjms2', objImgr2).then();
                        $scope.Detail.Tjms2 = $scope.Detail.Tjms2s[0];
                        $scope.Detail.Tjms2.CargoDescription = $scope.Detail.Tjms2.CargoDescription1 + ' ' + $scope.Detail.Tjms2.CargoDescription2 + ' ' + $scope.Detail.Tjms2.CargoDescription3;
                        $scope.Detail.Tjms2.Vessel = $scope.Detail.Tjms2.BargeName1 + ' ' + $scope.Detail.Tjms2.BargeName2 + ' ' + $scope.Detail.Tjms2.BargeName3;
                        $scope.Detail.Tjms2.StartDateTime = checkDateTimeisEmpty($scope.Detail.Tjms2.StartDateTime);
                        $scope.Detail.Tjms2.EndDateTime = checkDateTimeisEmpty($scope.Detail.Tjms2.EndDateTime);
                        $scope.Detail.Tjms2.DateCompleted = checkDateCompleted($scope.Detail.Tjms2.DateCompleted);
                        getSignature(objImgr2);
                        getTjms5(TrxNo);
                    }
                });

            });
        };
        GetTjms2s($scope.Detail.TrxNo);
    }
]);

app.controller('JoblistingConfirmCtrl', ['ENV', '$scope', '$state', '$stateParams', 'ApiService', '$ionicPopup', '$ionicPlatform', '$cordovaSQLite', '$cordovaNetwork', '$ionicLoading', 'SqlService', 'PopupService',
    function (ENV, $scope, $state, $stateParams, ApiService, $ionicPopup, $ionicPlatform, $cordovaSQLite, $cordovaNetwork, $ionicLoading, SqlService, PopupService) {
        var canvas = document.getElementById('signatureCanvas'),
            signaturePad = new SignaturePad(canvas),
            strEemptyBase64 = '';
        $scope.signature = null;
        $scope.Error = {
            Err1: '',
            Err2: '',
            Err3: ''
        };
        $scope.Confirm = {
            Tjms2: {
                TrxNo: $stateParams.TrxNo,
            }
        };
        $ionicPlatform.ready(function () {
            var strSqlFilter = "TrxNo='" + $scope.Confirm.Tjms2.TrxNo + "'  ";
            SqlService.Select('Tjms2', '*', strSqlFilter).then(function (results) {
                if (results.rows.length > 0) {
                    $scope.Confirm.Tjms2 = results.rows.item(0);
                    if ($scope.Confirm.Tjms2.TempBase64 !== null && is.not.empty($scope.Confirm.Tjms2.TempBase64)) {
                        if (is.not.equal(strEemptyBase64, $scope.Confirm.Tjms2.TempBase64)) {
                            $scope.signature = 'data:image/png;base64,' + $scope.Confirm.Tjms2.TempBase64;
                        }
                    }
                }
            });
        });

        function resizeCanvas() {
            var ratio = window.devicePixelRatio || 1;
            canvas.width = window.innerWidth - 50;
            canvas.height = screen.height / 3;
        }
        $scope.returnList = function () {
            $state.go('grList', {}, {});
        };
        $scope.returnDetail = function () {
            $state.go('grDetail', {
                'TrxNo': $scope.Confirm.Tjms2.TrxNo,
            }, {
                reload: true
            });
        };
        $scope.clearCanvas = function () {
            $scope.signature = null;
            signaturePad.clear();
        };
        $scope.saveCanvas = function () {
            var sigImg = signaturePad.toDataURL();
            if (is.not.equal(strEemptyBase64, sigImg)) {
                $scope.signature = sigImg;
            }
        };

        var showError = function () {
            if ($scope.Confirm.Tjms2.SignedByName.length <= 0) {
                $scope.Error.Err1 = "Name is not empty,";
            } else {
                $scope.Error.Err1 = "";
            }
            if ($scope.Confirm.Tjms2.SignedByNric.length <= 0) {
                $scope.Error.Err2 = "Nric is not empty ,";
            } else {
                $scope.Error.Err2 = "";
            }
            if ($scope.Confirm.Tjms2.SignedByDesignation.length <= 0) {
                $scope.Error.Err3 = "Designation is not empty ";
            } else {
                $scope.Error.Err3 = "";
            }
        };

        $scope.gotoPrint = function () {

            $state.go('grPrint', {
                'TrxNo': '1',
                'Flag': 'Y',
            }, {
                reload: true
            });
        };
        $scope.confirm = function () {
            showError();
            if ($scope.Error.Err1 === "" && $scope.Error.Err2 === "" && $scope.Error.Err3 === "") {
                $scope.saveCanvas();
                var signature = '';
                if (is.not.null($scope.signature)) {
                    signature = $scope.signature.split(',')[1];
                }
                var Tjms2Filter = "TrxNo='" + $scope.Confirm.Tjms2.TrxNo + "' and LineItemNo='" + $scope.Confirm.Tjms2.LineItemNo + "' "; // not record
                var objTjms2 = {
                    TempBase64: signature,
                };
                // var arrTjms2 = [];
                // arrTjms2.push($scope.Confirm.Tjms2);
                // var jsonData = {
                //     "UpdateAllString": JSON.stringify(arrTjms2)
                // };
                // var objUri = ApiService.Uri(true, '/api/tms/tjms2/update');
                // ApiService.Post(objUri, jsonData, true).then(function success(result) {});
                if ($scope.Confirm.Tjms2.ChargeBerthQty === "") {
                    $scope.Confirm.Tjms2.ChargeBerthQty = 0;
                }
                if ($scope.Confirm.Tjms2.ChargeLiftingQty === "") {
                    $scope.Confirm.Tjms2.ChargeLiftingQty = 0;
                }
                if ($scope.Confirm.Tjms2.SignalManQty === "") {
                    $scope.Confirm.Tjms2.SignalManQty = 0;
                }
                var objUriUpdate = ApiService.Uri(true, '/api/tms/tjms2/update');
                objUriUpdate.addSearch('TrxNo', $scope.Confirm.Tjms2.TrxNo);
                objUriUpdate.addSearch('LineItemNo', $scope.Confirm.Tjms2.LineItemNo);
                objUriUpdate.addSearch('SignedByName', $scope.Confirm.Tjms2.SignedByName);
                objUriUpdate.addSearch('SignedByNric', $scope.Confirm.Tjms2.SignedByNric);
                objUriUpdate.addSearch('SignedByDesignation', $scope.Confirm.Tjms2.SignedByDesignation);
                objUriUpdate.addSearch('strDateCompleted', $scope.Confirm.Tjms2.DateCompleted);
                objUriUpdate.addSearch('ChargeBerthQty', $scope.Confirm.Tjms2.ChargeBerthQty);
                objUriUpdate.addSearch('ChargeLiftingQty', $scope.Confirm.Tjms2.ChargeLiftingQty);
                objUriUpdate.addSearch('ChargeOther', $scope.Confirm.Tjms2.ChargeOther);
                objUriUpdate.addSearch('OfficeInChargeName', $scope.Confirm.Tjms2.OfficeInChargeName);
                objUriUpdate.addSearch('CompanyName', $scope.Confirm.Tjms2.CompanyName);
                objUriUpdate.addSearch('SignalManQty', $scope.Confirm.Tjms2.SignalManQty);
                ApiService.Get(objUriUpdate, false).then(function success(result) {});
                $ionicLoading.hide();

                SqlService.Update('Tjms2', objTjms2, Tjms2Filter).then(function (res) {});
                jsonData = {
                    'Base64': $scope.signature,
                    'FileName': $scope.Confirm.Tjms2.JobNo + '_' + $scope.Confirm.Tjms2.LineItemNo + '.Png'
                };
                if ($scope.signature !== null && is.not.equal($scope.signature, '') && is.not.undefined($scope.signature)) {
                    objUri = ApiService.Uri(true, '/api/tms/upload/img');
                    objUri.addSearch('Key', $scope.Confirm.Tjms2.JobNo);
                    objUri.addSearch('TableName', 'Tjms1');
                    ApiService.Post(objUri, jsonData, true).then(function success(result) {});
                }
                PopupService.Info(null, 'Confirm Success', '').then(function (res) {
                    $scope.returnList();
                });
            } else {
                PopupService.Info(null, $scope.Error.Err1 + $scope.Error.Err2 + $scope.Error.Err3, '').then(function (res) {

                });
            }
        };
        resizeCanvas();
        strEemptyBase64 = signaturePad.toDataURL();
    }
]);

app.controller('JoblistingPrintCtrl', ['ENV', '$scope', '$state', '$stateParams', 'ApiService', '$ionicPopup', '$ionicPlatform', '$cordovaSQLite', '$cordovaNetwork', '$ionicLoading', 'SqlService', 'PopupService',
    function (ENV, $scope, $state, $stateParams, ApiService, $ionicPopup, $ionicPlatform, $cordovaSQLite, $cordovaNetwork, $ionicLoading, SqlService, PopupService) {
        $scope.Flag = $stateParams.Flag;

        $scope.returnDetail = function () {
            if ($scope.Flag === 'N') {
                $state.go('grDetail', {
                    'TrxNo': '1',
                }, {
                    reload: true
                });
            } else {
                $state.go('jobListingConfirm', {
                    'TrxNo': '1',
                }, {
                    reload: true
                });
            }

        };

    }
]);

app.controller('Grtjms5Ctrl', ['ENV', '$scope', '$state', '$stateParams', 'ApiService', '$ionicPopup', '$ionicPlatform', '$cordovaSQLite', '$cordovaNetwork', '$ionicLoading', 'SqlService', 'PopupService',
    function (ENV, $scope, $state, $stateParams, ApiService, $ionicPopup, $ionicPlatform, $cordovaSQLite, $cordovaNetwork, $ionicLoading, SqlService, PopupService) {
        $scope.Flag = $stateParams.Flag;
        $scope.Detail = {
            tjms5: {
                TrxNo: $stateParams.TrxNo,
                EquipmentType: '',
                CargoDescription: '',
                ChargeWeight: 0,
                ChgWtRoundUp: 0,
                ContainerNo: '',
                EquipmentTypeDescription: '',
                disableVehicleType: false,
                VehicleNo: '',
                Volume: 0
            },
            tote1: {},
        };
        $scope.returnDetail = function () {
            $state.go('grDetail', {
                'TrxNo': $scope.Detail.tjms5.TrxNo,
            }, {
                reload: true
            });
        };
        $scope.ChgWtlost = function (ChgWeight) {
            if ('Flag' !== retrurnChgWtRoundUp(ChgWeight)) {
                $scope.Detail.tjms5.ChgWtRoundUp = retrurnChgWtRoundUp(ChgWeight);
            }
        };
        $scope.showTote1 = function (EquipmentType, LineItemNo) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1');
                objUri.addSearch('EquipmentType', EquipmentType.EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.tote1 = result.data.results;
                    if ($scope.Detail.tote1.length > 0) {
                        $scope.Detail.tjms5.EquipmentType = $scope.Detail.tote1[0].EquipmentType;
                        $scope.Detail.tjms5.EquipmentTypeDescription = $scope.Detail.tote1[0].EquipmentTypeDescription;
                        $scope.Detail.tjms5.Volume = $scope.Detail.tote1[0].Volume;
                        $scope.Detail.tjms5.ChargeWeight = $scope.Detail.tote1[0].ChgWt;
                        $scope.ChgWtlost($scope.Detail.tjms5.ChargeWeight);
                        // if ($scope.Detail.tote1[0].EditFlag === 'Y') {
                        //     $scope.Detail.tjms5.disabled = false;
                        // } else {
                        //     $scope.Detail.tjms5.disabled = true;
                        // }

                        if ($scope.Detail.tote1[0].EditFlag === 'Y') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        } else if ($scope.Detail.tote1[0].EditFlag === 'A') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = false;
                        } else {
                            $scope.Detail.tjms5.disabled = true;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        }
                    }

                });
            }
        };
        $scope.refreshEquipmentType = function (EquipmentType) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1/EquipmentType');
                objUri.addSearch('EquipmentType', EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.EquipmentTypes = result.data.results;
                });
            }
        };

        $scope.insertTjms5 = function () {
            if ($scope.Detail.tjms5.TrxNo > 0) {
                var objUri = ApiService.Uri(true, '/api/tms/tjms5/insert');
                objUri.addSearch('TrxNo', $scope.Detail.tjms5.TrxNo);
                objUri.addSearch('EquipmentType', $scope.Detail.tjms5.EquipmentType);
                objUri.addSearch('EquipmentTypeDescription', $scope.Detail.tjms5.EquipmentTypeDescription);
                objUri.addSearch('ContainerNo', $scope.Detail.tjms5.ContainerNo);
                objUri.addSearch('CargoDescription', $scope.Detail.tjms5.CargoDescription);
                objUri.addSearch('Volume', $scope.Detail.tjms5.Volume);
                objUri.addSearch('ChargeWeight', $scope.Detail.tjms5.ChargeWeight);
                objUri.addSearch('ChgWtRoundUp', $scope.Detail.tjms5.ChgWtRoundUp);
                objUri.addSearch('VehicleNo', $scope.Detail.tjms5.VehicleNo);
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.returnDetail();
                });

                // var arrtjms5 = [];
                // arrtjms5.push($scope.Detail.tjms5);
                // var jsonData = {
                //     "UpdateAllString": JSON.stringify(arrtjms5)
                // };
                // var objUri = ApiService.Uri(true, '/api/tms/tjms5/insert');
                // ApiService.Post(objUri, jsonData, true).then(function success(result) {
                //     $scope.returnDetail();
                // });

            }
        };

    }
]);

app.controller('grUpdateTjms5Ctrl', ['ENV', '$scope', '$state', '$stateParams', 'ApiService', '$ionicPopup', '$ionicPlatform', '$cordovaSQLite', '$cordovaNetwork', '$ionicLoading', 'SqlService', 'PopupService',
    function (ENV, $scope, $state, $stateParams, ApiService, $ionicPopup, $ionicPlatform, $cordovaSQLite, $cordovaNetwork, $ionicLoading, SqlService, PopupService) {

        $scope.Detail = {
            tjms5: {
                TrxNo: $stateParams.TrxNo,
                LineItemNo: $stateParams.LineItemNo,
                EquipmentType: '',
                CargoDescription: '',
                ChargeWeight: 0,
                ChgWtRoundUp: 0,
                ContainerNo: '',
                EquipmentTypeDescription: '',
                VehicleNo: '',
                disableVehicleType: false,
                Volume: 0
            },
            tote1: {},
        };
        $scope.returnDetail = function () {
            $state.go('grDetail', {
                'TrxNo': $scope.Detail.tjms5.TrxNo,
            }, {
                reload: true
            });
        };
        $scope.ChgWtlost = function (ChgWeight) {
            if ('Flag' !== retrurnChgWtRoundUp(ChgWeight)) {
                $scope.Detail.tjms5.ChgWtRoundUp = retrurnChgWtRoundUp(ChgWeight);
            }
        };
        $scope.getTjms5 = function () {
            if ($scope.Detail.tjms5.LineItemNo > 0) {
                var objUri = ApiService.Uri(true, '/api/tms/tjms5');
                objUri.addSearch('TrxNo', $scope.Detail.tjms5.TrxNo);
                objUri.addSearch('LineItemNo', $scope.Detail.tjms5.LineItemNo);
                ApiService.Get(objUri, true).then(function success(result) {
                    var results = result.data.results;
                    dataResults = new Array();
                    if (results.length > 0) {
                        // $scope.Detail.tjms5 = results;
                        var jobs = getjobs(results[0]);
                        $scope.showDetailTote1(results[0].EquipmentTypeDescription, $scope.Detail.tjms5.LineItemNo);
                        $scope.refreshEquipmentType(results[0].EquipmentType);
                        dataResults = dataResults.concat(jobs);
                        $scope.Detail.tjms5 = dataResults[0];
                    } else {
                        $scope.Detail.tjms5 = dataResults;
                    }
                });
            }
        };

        $scope.showDetailTote1 = function (EquipmentType, LineItemNo) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1');
                objUri.addSearch('EquipmentType', EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.tote1 = result.data.results;
                    if ($scope.Detail.tote1.length > 0) {
                        if ('Flag' !== retrurnChgWtRoundUp($scope.Detail.tjms5.ChargeWeight)) {
                            $scope.Detail.tjms5.ChgWtRoundUp = retrurnChgWtRoundUp($scope.Detail.tjms5.ChargeWeight);
                        }

                        if ($scope.Detail.tote1[0].EditFlag === 'Y') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        } else if ($scope.Detail.tote1[0].EditFlag === 'A') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = false;
                        } else {
                            $scope.Detail.tjms5.disabled = true;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        }
                    }

                });
            }
        };

        $scope.showTote1 = function (EquipmentType, LineItemNo) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1');
                objUri.addSearch('EquipmentType', EquipmentType.EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.Detail.tote1 = result.data.results;
                    if ($scope.Detail.tote1.length > 0) {
                        $scope.Detail.tjms5.EquipmentType = $scope.Detail.tote1[0].EquipmentType;
                        $scope.Detail.tjms5.EquipmentTypeDescription = $scope.Detail.tote1[0].EquipmentTypeDescription;
                        $scope.Detail.tjms5.Volume = $scope.Detail.tote1[0].Volume;
                        $scope.Detail.tjms5.ChargeWeight = $scope.Detail.tote1[0].ChgWt;

                        if ($scope.Detail.tote1[0].EditFlag === 'Y') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        } else if ($scope.Detail.tote1[0].EditFlag === 'A') {
                            $scope.Detail.tjms5.disabled = false;
                            $scope.Detail.tjms5.disableVehicleType = false;
                        } else {
                            $scope.Detail.tjms5.disabled = true;
                            $scope.Detail.tjms5.disableVehicleType = true;
                        }
                    }

                });
            }
        };
        $scope.refreshEquipmentType = function (EquipmentType) {
            if (is.not.undefined(EquipmentType) && is.not.empty(EquipmentType)) {
                var objUri = ApiService.Uri(true, '/api/tms/Tovt1/EquipmentType');
                objUri.addSearch('EquipmentType', EquipmentType);
                ApiService.Get(objUri, false).then(function success(result) {
                    $scope.EquipmentTypes = result.data.results;
                });
            }
        };

        var getjobs = function (obj) {
            var jobs = {
                TrxNo: obj.TrxNo,
                LineItemNo: obj.LineItemNo,
                disabled: false,
                EquipmentType: obj.EquipmentType,
                EquipmentTypeDescription: obj.EquipmentTypeDescription,
                ContainerNo: obj.ContainerNo,
                CargoDescription: obj.CargoDescription,
                Volume: obj.Volume,
                ChargeWeight: obj.ChargeWeight,
                ChgWtRoundUp: obj.ChgWtRoundUp,
                VehicleNo: obj.VehicleNo,

            };
            return jobs;
        };
        $scope.getTjms5();

        $scope.updateTjms5 = function () {
            if ($scope.Detail.tjms5.TrxNo > 0) {
                var objUri = ApiService.Uri(true, '/api/tms/tjms5/update');
                objUri.addSearch('TrxNo', $scope.Detail.tjms5.TrxNo);
                objUri.addSearch('LineItemNo', $scope.Detail.tjms5.LineItemNo);
                objUri.addSearch('EquipmentType', $scope.Detail.tjms5.EquipmentType);
                objUri.addSearch('EquipmentTypeDescription', $scope.Detail.tjms5.EquipmentTypeDescription);
                objUri.addSearch('ContainerNo', $scope.Detail.tjms5.ContainerNo);
                objUri.addSearch('CargoDescription', $scope.Detail.tjms5.CargoDescription);
                objUri.addSearch('Volume', $scope.Detail.tjms5.Volume);
                objUri.addSearch('ChargeWeight', $scope.Detail.tjms5.ChargeWeight);
                objUri.addSearch('ChgWtRoundUp', $scope.Detail.tjms5.ChgWtRoundUp);
                objUri.addSearch('VehicleNo', $scope.Detail.tjms5.VehicleNo);
                ApiService.Get(objUri, true).then(function success(result) {
                    $scope.returnDetail();
                });
            }
        };
    }
]);
