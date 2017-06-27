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
        var hmImgr2 = new HashMap();
        var hmImsn1 = new HashMap();
        $scope.Detail = {
            TrxNo: $stateParams.TrxNo,
            Tjms2: {
                CargoDescription: '',
                Vessel: '',
            },
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

        var UpdateTjms2 =function(){
          var Tjms2Filter = "TrxNo='" + $scope.Detail.Tjms2.TrxNo + "' and LineItemNo='" + $scope.Detail.Tjms2.LineItemNo + "' ";
          var objTjms2 = {
              DateCompleted:$scope.Detail.Tjms2.DateCompleted
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

        $scope.gotoConfirm = function () {
          UpdateTjms2();
            $state.go('jobListingConfirm', {
                'TrxNo': $scope.Detail.TrxNo,

            }, {
                reload: true
            });
        };
        var getSignature = function (objTjms2) {
            var objUri = ApiService.Uri(true, '/api/tms/tjms2/attach');
            objUri.addSearch('Key', objTjms2.TrxNo);
            objUri.addSearch('LineItemNo', objTjms2.LineItemNo);
            objUri.addSearch('TableName', 'Tjms');
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
        $scope.confirm = function () {
            $scope.saveCanvas();
            var signature = '';
            if (is.not.null($scope.signature)) {
                signature = $scope.signature.split(',')[1];
            }
            var Tjms2Filter = "TrxNo='" + $scope.Confirm.Tjms2.TrxNo + "' and LineItemNo='" + $scope.Confirm.Tjms2.LineItemNo + "' "; // not record
            var objTjms2 = {
                TempBase64: signature,
            };
            var arrTjms2 = [];
            arrTjms2.push($scope.Confirm.Tjms2);
            var jsonData = {
                "UpdateAllString": JSON.stringify(arrTjms2)
            };
            var objUri = ApiService.Uri(true, '/api/tms/tjms2/update');
            ApiService.Post(objUri, jsonData, true).then(function success(result) {});
            SqlService.Update('Tjms2', objTjms2, Tjms2Filter).then(function (res) {});
            jsonData = {
                'Base64': $scope.signature,
                'FileName': $scope.Confirm.Tjms2.TrxNo + '_' + $scope.Confirm.Tjms2.LineItemNo + '.Png'
            };
            if ($scope.signature !== null && is.not.equal($scope.signature, '') && is.not.undefined($scope.signature)) {
                objUri = ApiService.Uri(true, '/api/tms/upload/img');
                objUri.addSearch('Key', $scope.Confirm.Tjms2.TrxNo);
                objUri.addSearch('TableName', 'Tjms');
                ApiService.Post(objUri, jsonData, true).then(function success(result) {});
            }
            PopupService.Info(null, 'Confirm Success', '').then(function (res) {
                $scope.returnList();
            });
        };
        resizeCanvas();
        strEemptyBase64 = signaturePad.toDataURL();
    }
]);
