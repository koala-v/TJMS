using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
   public class tjms2
    {
        public string JobNo { get; set; }
        public int TrxNo { get; set; }
        public int LineItemNo { get; set; }
        public decimal Pcs { get; set; }
        public decimal GrossWeight { get; set; }
        public string CargoDescription1 { get; set; }
        public string CargoDescription2 { get; set; }
        public string CargoDescription3 { get; set; }
        public string BargeName1 { get; set; }
        public string BargeName2 { get; set; }
        public string BargeName3 { get; set; }
        public DateTime DateCompleted { get; set; }
        public string ContainerNo1 { get; set; }
        public string ContainerType1 { get; set; }
        public int Pcs1 { get; set; }
        public string VehicleNo1 { get; set; }
        public string ContainerNo2 { get; set; }
        public string ContainerType2 { get; set; }
        public int Pcs2 { get; set; }
        public string VehicleNo2 { get; set; }
        public string ContainerNo3 { get; set; }
        public string ContainerType3 { get; set; }
        public int Pcs3{ get; set; }
        public string VehicleNo3{ get; set; }
        public DateTime StartDateTime { get; set; } 
        public DateTime EndDateTime { get; set; }
        public int ChargeBerthQty { get; set; }
        public int ChargeLiftingQty { get; set; }
        public string ChargeOther { get; set; }
        public string SignedByName { get; set; }
        public string SignedByNric { get; set; }
        public string SignedByDesignation { get; set; }
        public string CompanyName { get; set; }


    }
}
