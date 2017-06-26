var appFactory = angular.module('WMSAPP.factories', []);

appFactory.factory('TABLE_DB', function () {
    var TABLE_DB = {
        Tjms2: {
            JobNo: 'TEXT',
            TrxNo: 'INT',
            LineItemNo: 'INT',
            Pcs: 'INT',
            GrossWeight: 'INT',
            CargoDescription1: 'TEXT',
            CargoDescription2: 'TEXT',
            CargoDescription3: 'TEXT',
            BargeName1: 'TEXT',
            BargeName2: 'TEXT',
            BargeName3: 'TEXT',
            DateCompleted: 'TEXT',
            ContainerNo1: 'TEXT',
            ContainerType1: 'TEXT',
            Pcs1: 'INT',
            VehicleNo1: 'TEXT',
            ContainerNo2: 'TEXT',
            ContainerType2: 'TEXT',
            Pcs2: 'INT',
            VehicleNo2: 'TEXT',
            ContainerNo3: 'TEXT',
            ContainerType3: 'TEXT',
            Pcs3: 'INT',
            VehicleNo3: 'TEXT',
            StartDateTime: 'TEXT',
            EndDateTime: 'TEXT',
            ChargeBerthQty: 'INT',
            ChargeLiftingQty: 'INT',
            ChargeOther: 'TEXT',
            SignedByName: 'TEXT',
            SignedByNric: 'TEXT',
            SignedByDesignation: 'TEXT',
            CompanyName: 'TEXT',
            TempBase64:'TEXT',
        },
    };
    return TABLE_DB;
});