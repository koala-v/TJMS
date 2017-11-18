using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
  public  class Tjms5
    {
         public int TrxNo { get; set; }
        public  int LineItemNo { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentTypeDescription { get; set; }
        public string ContainerNo { get; set; }
        public string CargoDescription { get; set; }
        public decimal Volume { get; set; }
        public decimal ChargeWeight { get; set; }
        public decimal ChgWtRoundUp { get; set; }
        public string VehicleNo { get; set; }
         public string disabled { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
