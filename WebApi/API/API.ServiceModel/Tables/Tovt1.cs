using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApi.ServiceModel.Tables
{
   public  class Tovt1
    {
        public int LineItemNo { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentTypeDescription { get; set; }         
        public string EditFlag { get; set; }
        public decimal Volume { get; set; }
        public decimal ChgWt { get; set; }

    }
}
