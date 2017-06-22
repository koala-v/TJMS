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
        $scope.showDate = function (utc) {
            return moment(utc).format('DD-MMM-YYYY');
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
        SqlService,
        ApiService,
        PopupService) {
        var popup = null;
        var hmImgr2 = new HashMap();
        var hmImsn1 = new HashMap();
        $scope.Detail = {
            TrxNo: $stateParams.TrxNo,
            Tjms2: {},
            Tjms2s: {},
        };
        $scope.returnList = function () {
            if ($ionicHistory.backView()) {
                $ionicHistory.goBack();
            } else {
                $state.go('grList', {}, {
                    reload: false
                });
            }
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
                }
                });
            });
        };
        GetTjms2s($scope.Detail.TrxNo);
    }
]);
