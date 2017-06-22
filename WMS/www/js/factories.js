var appFactory = angular.module('WMSAPP.factories', []);

appFactory.factory('TABLE_DB', function () {
    var TABLE_DB = {
      Tjms2 : {
        TrxNo: 'INT',
              LineItemNo: 'INT',
      },
    };
    return TABLE_DB;
});
