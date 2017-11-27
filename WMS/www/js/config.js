'use strict';
var appConfig = angular.module('WMSAPP.config',[]);
appConfig.constant('ENV', {
    website:      'www.sysfreight.net/app/wms/tjms',
   // api:          'www.sysfreight.net/apis/wms/tjms',
   api:        'localhost:2354',
    reset:  {
        website:      'www.sysfreight.net/app/wms/tjms',
        api:          'www.sysfreight.net/apis/wms/tjms',
       port:         '8081'
    },
    ssl:          false, // 0 : false, 1 : true
    // port:         '8081', // http port no
    debug:        true,
    mock:         false,
    fromWeb:      true,
    websql : {
        name: 'TJMSDB',
        version: '1.0',
        displayName: 'WMS Database',
        estimatedSize: 10 * 11024 * 1024
    },
    sqlite : {
        name: 'AppTJMS.db',
        location: 'default'
    },
    appId:        '9CBA0A78-7D1D-49D3-BA71-C72E93F9E48F',
    apkName:      'TJMS',
    updateFile:   'update.json',
    rootPath:     'TJMSPath',
    configFile:   'config.txt',
    version:      '1.0.1.5',
    parameter: {
        showSerialNo : false
    },
    apiMap: {
        login: {
            check : '/api/tms/login/check'
        }
    }
});
