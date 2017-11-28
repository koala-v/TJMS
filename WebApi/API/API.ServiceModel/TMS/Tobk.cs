using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.OrmLite;
using WebApi.ServiceModel.Tables;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApi.ServiceModel.TMS
{
    [Route("/tms/tjms2", "Get")]  //DriverCode=
    [Route("/tms/tjms5", "Get")] // TrxNo=
    [Route("/tms/tjms2/update", "Get")] //
    [Route("/tms/tjms2/confirm", "Get")] //update?Key=,Remark=,TableName=
    [Route("/tms/tjms2/PickupTimeUpdate", "Post")] 
    [Route("/tms/toet1/EquipmentType", "Get")]
    [Route("/tms/toet1", "Get")]
    [Route("/tms/tjms5/update", "Post")]
    [Route("/tms/tjms5/insert", "Post")]
    [Route("/tms/tjms5/delete", "get")]


    public class Tobk : IReturn<CommonResponse>
    {
        public string TrxNo { get; set; }
        public string Key { get; set; }
        public string LineItemNo { get; set; }
        public string Remark { get; set; }
        public string OnBehalfName { get; set; }
        public string TableName { get; set; }
        public string UpdateAllString { get; set; }
        public string DriverCode { get; set; }
        public string BookingNo { get; set; }
        public string JobNo { get; set; }
        public string DCDescription { get; set; }
        public string ScheduleDateFlag { get; set; }
        //  tjms start
        public string SignedByName { get; set; }
        public string SignedByNric { get; set; }
        public string SignedByDesignation { get; set; }
        public string strDateCompleted { get; set; }
        public string ChargeBerthQty { get; set; }
        public string ChargeOther { get; set; }
        public string ChargeLiftingQty { get; set; }
        public string OfficeInChargeName { get; set; }
        public string CompanyName { get; set; }
        public string SignalManQty { get; set; }

        // tjms end
        public string EquipmentType { get; set; }
}
    public class Tobk_Logic
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        public List<tjms2> Get_tjms2_List(Tobk request)
        {

            List<tjms2> Result = null;
            try
            {                
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    string strSql = "";
                    strSql = " select isnull((select tjms1.JobNo from tjms1 where trxno = tjms2.TrxNo),'') as JobNo ," +
                                " tjms2.TrxNo, " +
                                " tjms2.LineItemNo," +
                                " TJMS2.Pcs," +
                                " TJMS2.GrossWeight," +
                                " isnull(TJMS2.CargoDescription1, '') as CargoDescription1," +
                                " isnull(TJMS2.CargoDescription2, '') as CargoDescription2," +
                                " isnull(TJMS2.CargoDescription3, '') as CargoDescription3," +
                                 " isnull(TJMS2.OfficeInChargeName, '') as OfficeInChargeName," +
                                " isnull((select tjms3.BargeName from tjms3 where trxno = tjms2.TrxNo and LineItemNo = 1) ,'') as BargeName1 ," +
                                " isnull((select tjms3.BargeName from tjms3 where trxno = tjms2.TrxNo and LineItemNo = 2) ,'') as BargeName2 ," +
                                " isnull((select tjms3.BargeName from tjms3 where trxno = tjms2.TrxNo and LineItemNo = 3) ,'') as BargeName3 ," +                   
                                " (select tjms1.DateCompleted from tjms1 where trxno = tjms2.TrxNo) as DateCompleted," +
                                " isnull(TJMS2.ContainerNo1, '') as ContainerNo1," +
                                " isnull(TJMS2.ContainerType1, '') as ContainerType1," +
                                " tjms2.Pcs1 as Pcs1," +
                                " isnull(TJMS2.CargoDescription1, '') as CargoDescription1, " +
                                " isnull(TJMS2.VehicleNo1, '') as VehicleNo1," +
                                " (select tjms4.StartDateTime from tjms4 where tjms4.trxno = tjms2.TrxNo and tjms4.LineItemNo = tjms2.lineItemNo) as StartDateTime," +
                                " (select tjms4.EndDateTime from tjms4 where tjms4.trxno = tjms2.TrxNo and tjms4.LineItemNo = tjms2.lineItemNo) as EndDateTime," +
                                " isnull(TJMS2.ContainerNo2, '') as ContainerNo2," +
                                " isnull(TJMS2.ContainerType2, '') as ContainerType2," +
                                " tjms2.Pcs2 as Pcs2," +
                                " isnull(TJMS2.CargoDescription2, '') as CargoDescription2," +
                                " isnull(TJMS2.VehicleNo2, '') as VehicleNo2," +
                                " isnull(TJMS2.ContainerNo3, '') as ContainerNo3," +
                                " isnull(TJMS2.ContainerType3, '') as ContainerType3," +
                                " tjms2.Pcs3 as Pcs3," +
                                " isnull(TJMS2.CargoDescription3, '') as CargoDescription3," +
                                " isnull(TJMS2.VehicleNo3, '') as VehicleNo3," +
                                " (select tjms1.ChargeBerthQty from tjms1 where trxno = tjms2.TrxNo) as ChargeBerthQty," +
                                " (select tjms1.ChargeLiftingQty from tjms1 where trxno = tjms2.TrxNo) as ChargeLiftingQty," +
                                " (select tjms1.SignalManQty from tjms1 where trxno = tjms2.TrxNo) as SignalManQty," +
                                " isnull((select tjms1.ChargeOther from tjms1 where trxno = tjms2.TrxNo),'') as ChargeOther," +
                                " isnull((select tjms1.SignedByName from tjms1 where trxno = tjms2.TrxNo),'') as SignedByName," +
                                " isnull((select tjms1.SignedByNric from tjms1 where trxno = tjms2.TrxNo),'') as SignedByNric," +
                                " isnull((select tjms1.SignedByDesignation from tjms1 where trxno = tjms2.TrxNo),'') as SignedByDesignation," +
                                " (select tjms1.CustomerSignOffDateTime from tjms1 where trxno = tjms2.TrxNo) as CustomerSignOffDateTime, " +
                                " isnull((select CompanyName from saco1),'') as CompanyName" +
                                "  from tjms2  where trxno = " + request.TrxNo + " ";      
                    Result = db.Select<tjms2>(strSql);
                }
            }
            catch { throw; }
            return Result;

        }


        public int DeleteLineItem(Tobk request)
        {

            int Result = -1;

            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.TrxNo != null && request.TrxNo != "" && request.LineItemNo != null && request.LineItemNo != "")
                    {
                        string strSql = "";
                        strSql = "Delete from Tjms5 where " +
                          "TrxNo ='" + request.TrxNo + "'" +
                          "And LineItemNo=" + request.LineItemNo + "";
                        db.ExecuteSql(strSql);                  
                    }

                }
                Result = 1;

            }
            catch { throw; }
            return Result;
        }

        public List<Tjms5> Get_tjms5_List(Tobk request)
        {
            List<Tjms5> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {
                    string strSql = "";
                    strSql =
                            "  select ISNULL( EquipmentType,'') as EquipmentType , " +
                            "  LineItemNo ," +
                             " TrxNo , "+
                            " ISNULL( EquipmentTypeDescription,'') as EquipmentTypeDescription," +
                            " ISNULL(ContainerNo,'') as ContainerNo ," +
                            "  ISNULL(CargoDescription ,'') as CargoDescription," +
                            " Volume ," +
                            " ChargeWeight ," +
                            " ChgWtRoundUp," +
                            " ISNULL(VehicleNo ,'') as VehicleNo," +
                            " '' as disabled  ," +
                            " (select Top 1  StartDateTime from tjms4 where tjms4.TrxNo = tjms5.TrxNo  ) as StartDateTime," +
                            " (select Top 1  EndDateTime from tjms4 where tjms4.TrxNo = tjms5.TrxNo  ) as EndDateTime " +
                            " from tjms5 " +
                            "where trxno = " + request.TrxNo + "";       
                    Result = db.Select<Tjms5>(strSql);
                }
            }
            catch { throw; }
            return Result;

        }
        public int confirm_tjms2(Tobk request)
        {
            int Result = -1;
         
            return Result;
        }

        public int updatePickupTime(Tobk request) {
            int Result = -1;
      
            return Result;
        }

        public List<Toet1> Get_EquipmentType(Tobk request)
        {
            List<Toet1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {

                    if (!string.IsNullOrEmpty(request.EquipmentType))
                    {
                        if (!string.IsNullOrEmpty(request.EquipmentType))
                        {

                            //string strSQL = "select DISTINCT CustomerCode as BusinessPartyCode ,CustomerName as  BusinessPartyName  from tjms1 where IsNUll(StatusCode,'')<>'DEL' And CustomerCode LIKE '" + request.BusinessPartyName + "%' Order By tjms1.CustomerCode Asc";

                            string strSQL = "Select EquipmentType From toet1 Where EquipmentType LIKE '" + request.EquipmentType + "%'";
                            Result = db.Select<Toet1>(strSQL);
                        }

                    }

                }

            }
            catch { throw; }
            return Result;

        }

        public List<Toet1> list_toet1(Tobk request)
        {
            List<Toet1> Result = null;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection("TMS"))
                {

                    if (!string.IsNullOrEmpty(request.EquipmentType))
                    {
                        if (!string.IsNullOrEmpty(request.EquipmentType))
                        {
                            string strSQL = "Select ISNULL(EquipmentType,'') as EquipmentType, ISNULL(EquipmentTypeDescription,'') as EquipmentTypeDescription,Volume,ChgWt,ISNULL(EditFlag,'') as EditFlag From toet1 Where EquipmentType = '" + request.EquipmentType + "'";
                            Result = db.Select<Toet1>(strSQL);
                        }

                    }

                }

            }
            catch { throw; }
            return Result;

        }


        public int insert_tjms5(Tobk request)
        {

            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {
                            for (int i = 0; i < ja.Count(); i++)
                            {

                                if (ja[i]["TrxNo"] == null || ja[i]["TrxNo"].ToString() == "")
                                {
                                    continue;
                                }
                                int TrxNo = Modfunction.ReturnZero(ja[i]["TrxNo"].ToString());
                                string EquipmentType = Modfunction.SQLSafeValue(Modfunction.CheckNull(ja[i]["EquipmentType"]));
                                string EquipmentTypeDescription = Modfunction.SQLSafeValue(Modfunction.CheckNull(ja[i]["EquipmentTypeDescription"]));
                                string ContainerNo =Modfunction.SQLSafeValue( Modfunction.CheckNull(ja[i]["ContainerNo"]));
                                string CargoDescription = Modfunction.SQLSafeValue(Modfunction.CheckNull(ja[i]["CargoDescription"]));
                                int Volume = Modfunction.ReturnZero(ja[i]["Volume"].ToString());
                                int ChargeWeight = Modfunction.ReturnZero(ja[i]["ChargeWeight"].ToString());
                                int ChgWtRoundUp = Modfunction.ReturnZero(Modfunction.CheckNull(ja[i]["ChgWtRoundUp"]));                  
                                string VehicleNo = Modfunction.SQLSafeValue(Modfunction.CheckNull(ja[i]["VehicleNo"]));
                                string startDateTime = "(select  Top 1 ISNULL(StartDateTime,NULL) AS StartDateTime  from tjms4 where TrxNo=" + TrxNo + ")  ";
                                string endDateTime = "(select  Top 1  ISNULL(EndDateTime,NULL) AS EndDateTime  from tjms4 where TrxNo=" + TrxNo + ")";
                                string strSql = "";



                                int intMaxLineItemNo = 1;
                                List<Tjms5> list1 = db.Select<Tjms5>("Select Max(LineItemNo) LineItemNo from Tjms5 Where TrxNo = " + TrxNo);
                                if (list1 != null)
                                {
                                    if (list1[0].LineItemNo > 0)
                                        intMaxLineItemNo = list1[0].LineItemNo + 1;
                                }


                                if (intMaxLineItemNo != 0)
                                {

                                    strSql = "insert into tjms5 (" +
                                        " TrxNo ," +
                                        " LineItemNo ," +
                                        " EquipmentType ," +
                                        " EquipmentTypeDescription  ," +
                                        " ContainerNo ," +
                                        " CargoDescription  ," +
                                        " Volume ," +
                                        " ChargeWeight ," +
                                        " ChgWtRoundUp ," +
                                        " VehicleNo  " +
                                     
                                        "  )" +
                                              "values( " +
                                              TrxNo + " , " +
                                        intMaxLineItemNo + " , " +
                                        EquipmentType + " , " +
                                        EquipmentTypeDescription + " , " +
                                        ContainerNo + " , " +
                                        CargoDescription + " , " +
                                        Volume + " , " +
                                        ChargeWeight + " , " +
                                        ChgWtRoundUp + " , " +
                                        VehicleNo + "  " +
                                      
                                            ") ";
                                    db.ExecuteSql(strSql);
                                

                                }

                            }
                        }
                        Result = 1;

                    }
                }
            }
            catch { throw; }
            return Result;
        }

        public int UpdateAll_tjms5(Tobk request)
        {

            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    if (request.UpdateAllString != null && request.UpdateAllString != "")
                    {
                        JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                        if (ja != null)
                        {
                            for (int i = 0; i < ja.Count(); i++)
                            {

                                if (ja[i]["TrxNo"] == null || ja[i]["TrxNo"].ToString() == "")
                                {
                                    continue;
                                }
                                int TrxNo = Modfunction.ReturnZero(ja[i]["TrxNo"].ToString());
                                string EquipmentType = Modfunction.CheckNull(ja[i]["EquipmentType"]);                        
                                string EquipmentTypeDescription = Modfunction.CheckNull(ja[i]["EquipmentTypeDescription"]);
                                string ContainerNo = Modfunction.CheckNull(ja[i]["ContainerNo"]);
                                string CargoDescription = Modfunction.CheckNull(ja[i]["CargoDescription"]);
                                int Volume = Modfunction.ReturnZero(ja[i]["Volume"].ToString ());
                                int ChargeWeight = Modfunction.ReturnZero(ja[i]["ChargeWeight"].ToString());
                                int ChgWtRoundUp = Modfunction.ReturnZero(ja[i]["ChgWtRoundUp"].ToString());
                                int LineItemNo = Modfunction.ReturnZero(ja[i]["LineItemNo"].ToString());
                                string VehicleNo = Modfunction.CheckNull(ja[i]["VehicleNo"]);

                                string strSql="";
                                if (LineItemNo != 0)
                                {
                                    strSql = "Update tjms5 set " +
                                        "EquipmentType='" + EquipmentType + "' , " +
                                         "EquipmentTypeDescription='" + EquipmentTypeDescription + "' ," +
                                         "ContainerNo='" + ContainerNo + "' , " +
                                         "CargoDescription='" + CargoDescription + "' , " +
                                         "Volume=" + Volume + " ," +
                                         "ChargeWeight=" + ChargeWeight + " , " +
                                          "ChgWtRoundUp=" + ChgWtRoundUp + ",  " +
                                         " VehicleNo='" + VehicleNo + "' " +
                                          "Where LineItemNo =" + LineItemNo + " And TrxNo=" + TrxNo + "";
                                    db.ExecuteSql(strSql);
                               
                                }

                            }
                        }
                        Result = 1;

                    }
                }
            }
            catch { throw; }
            return Result;
        }

        public int UpdateAll_tjms2(Tobk request)
        {
            int Result = -1;
            try
            {
                using (var db = DbConnectionFactory.OpenDbConnection())
                {
                    //if (request.UpdateAllString != null && request.UpdateAllString != "")
                    //{
                    //    JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
                    //    if (ja != null)
                    //    {
                    //        for (int i = 0; i < ja.Count(); i++)
                    //        {
                    //            int ChargeBerthQty;
                    //            int ChargeLiftingQty;
                    //            if (ja[i]["TrxNo"] == null || ja[i]["TrxNo"].ToString() == "")
                    //            { continue; }
                    //            string strTrxNo = ja[i]["TrxNo"].ToString();
                    //            string strLineItemNo = ja[i]["LineItemNo"].ToString();
                    //            string SignedByName = ja[i]["SignedByName"].ToString();
                    //            string SignedByNric = ja[i]["SignedByNric"].ToString();
                    //            string SignedByDesignation = ja[i]["SignedByDesignation"].ToString();
                    //            string CompanyName = ja[i]["CompanyName"].ToString();
                    //            string strDateCompleted = ja[i]["DateCompleted"].ToString();
                    //            string OfficeInChargeName = ja[i]["OfficeInChargeName"].ToString();
                        //        if (request .ChargeBerthQty.ToString() == "")
                        //        {
                        //request.ChargeBerthQty = 0;
                        //        }
                        //        else
                        //        {
                        //request.ChargeBerthQty = int.Parse(ja[i]["ChargeBerthQty"].ToString());
                        //        }

                        //        if (ja[i]["ChargeLiftingQty"].ToString() == "")
                        //        {
                        //            ChargeLiftingQty = 0;
                        //        }
                        //        else
                        //        {
                        //            ChargeLiftingQty = int.Parse(ja[i]["ChargeLiftingQty"].ToString());
                        //        }

                        //        string ChargeOther = ja[i]["ChargeOther"].ToString();
                                DateTime dt = DateTime.Now;
                                //if (strDateCompleted != "" && strDateCompleted != null) {
                                //    strDateCompleted = strDateCompleted +" "+ dt.GetDateTimeFormats('t')[0].ToString();
                                //}
                                string str;
                                if (request.LineItemNo  != "0")
                                {

                        str = "AttachmentFlag='Y' , CustomerSignOffDateTime=GetDate() ,  SignedByName = " + Modfunction.SQLSafeValue(request.SignedByName) + ",SignedByNric= " + Modfunction.SQLSafeValue(request.SignedByNric) + ",SignedByDesignation= " + Modfunction.SQLSafeValue(request.SignedByDesignation) + ",DateCompleted=" + Modfunction.SQLSafeValue(request.strDateCompleted) +  ",ChargeBerthQty="+ request.ChargeBerthQty + ",ChargeLiftingQty="+request.ChargeLiftingQty + ",SignalManQty=" + request.SignalManQty + ", ChargeOther=" + Modfunction.SQLSafeValue(request.ChargeOther) + "";
                                    db.Update("tjms1",
                                           str,
                                           " TrxNo='" + request.TrxNo + "' ");

                                    str =  "OfficeInChargeName= " + Modfunction.SQLSafeValue(request.OfficeInChargeName) + "";
                                    db.Update("tjms2",
                                           str,
                                           " TrxNo='" + request.TrxNo + "' ");

                                    str = " CompanyName = " + Modfunction.SQLSafeValue(request.CompanyName) + "";
                                    db.Update("saco1",
                                           str
                                           );
                                }

                            Result = 1;
                           
                }
            }
            catch { throw; }
            return Result;
        }

    }
}
