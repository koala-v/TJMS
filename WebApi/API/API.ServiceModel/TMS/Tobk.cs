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
    [Route("/tms/tjms2/update", "Post")] //
    [Route("/tms/tjms2/confirm", "Get")] //update?Key=,Remark=,TableName=
    [Route("/tms/tjms2/PickupTimeUpdate", "Post")] 
    

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
                                " isnull((select tjms1.ChargeOther from tjms1 where trxno = tjms2.TrxNo),'') as ChargeOther," +
                                " isnull((select tjms1.SignedByName from tjms1 where trxno = tjms2.TrxNo),'') as SignedByName," +
                                " isnull((select tjms1.SignedByNric from tjms1 where trxno = tjms2.TrxNo),'') as SignedByNric," +
                                " isnull((select tjms1.SignedByDesignation from tjms1 where trxno = tjms2.TrxNo),'') as SignedByDesignation," +
                                " isnull((select CompanyName from saco1),'') as CompanyName" +
                                "  from tjms2  where trxno = " + request.TrxNo + " ";      
                    Result = db.Select<tjms2>(strSql);
                }
            }
            catch { throw; }
            return Result;

        }
        public int confirm_tjms2(Tobk request)
        {
            int Result = -1;
            //try
            //{
            //    using (var db = DbConnectionFactory.OpenDbConnection())
            //    {
            //        string strJobNo = request.JobNo;
            //        if (strJobNo != "" && strJobNo != null )
            //        {
            //            int intMaxLineItemNo = 1;
            //            List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
            //            if (list1 != null)
            //            {
            //                if (list1[0].LineItemNo > 0)
            //                    intMaxLineItemNo = list1[0].LineItemNo + 1;
            //            }
            //            if (request.DCDescription == "Collection")
            //            {
            //                request.DCDescription = "COLLECTED";
            //            }
            //            else
            //            {
            //                request.DCDescription = "DELIVERED";
            //            }
            //            db.Insert(new tjms3
            //            {
            //                JobNo = Modfunction.SQLSafe(strJobNo),
            //                DateTime = DateTime.Now,
            //                UpdateDatetime = DateTime.Now,
            //                LineItemNo = intMaxLineItemNo,
            //                RefNo = Modfunction.SQLSafe(request.LineItemNo).ToString(),
            //                AutoFlag = "N",
            //                StatusCode = "POD",
            //                UpdateBy = Modfunction.SQLSafe(request.DriverCode),
            //                Remark = Modfunction.SQLSafeValue(request.Remark),
            //                Description = Modfunction.SQLSafe(request.DCDescription)
            //            });
                     
            //        }


            //        string str;
            //        if( request.LineItemNo != "0")
            //         {
            //            str = " Note = " + Modfunction.SQLSafeValue(request.Remark) + ",DeliveryDate=GETDATE(),CompleteFlag='Y'";
            //            db.Update(request.TableName,
            //                   str,
            //                   " BookingNo='" + request.Key + "' and LineItemNo = '" + request.LineItemNo + "' ");
            //        } else {
            //            str = " Note = " + Modfunction.SQLSafeValue(request.Remark) + ",DeliveryEndDateTime=GETDATE(),StatusCode = 'POD',CompletedFlag='Y'";
            //            db.Update("tjms2",
            //                   str,
            //                   " BookingNo='" + request.Key + "'");
            //        }

            //    }

            //}
            //catch { throw; }
            return Result;
        }

        public int updatePickupTime(Tobk request) {
            int Result = -1;
            //try
            //{
            //    using (var db = DbConnectionFactory.OpenDbConnection())
            //    {
            //        if (request.UpdateAllString != null && request.UpdateAllString != "")
            //        {
            //            JArray ja = (JArray)JsonConvert.DeserializeObject(request.UpdateAllString);
            //            if (ja != null)
            //            {
            //                for (int i = 0; i < ja.Count(); i++)
            //                {
                              
            //                    string strKey = ja[i]["Key"].ToString();
            //                    string strTobk2LineItemNo = ja[i]["LineItemNo"].ToString();              
            //                        string strJobNo = "";
            //                        if (ja[i]["JobNo"] != null || ja[i]["JobNo"].ToString() != "")
            //                            strJobNo = ja[i]["JobNo"].ToString();
            //                        if (strJobNo != "")
            //                        {
            //                            int intMaxLineItemNo = 1;
            //                            List<tjms3> list1 = db.Select<tjms3>("Select Max(LineItemNo) LineItemNo from tjms3 Where JobNo = " + Modfunction.SQLSafeValue(strJobNo));
            //                            if (list1 != null)
            //                            {
            //                                if (list1[0].LineItemNo > 0)
            //                                    intMaxLineItemNo = list1[0].LineItemNo + 1;
            //                            }
            //                            db.Insert(new tjms3
            //                            {
            //                                JobNo = strJobNo,
            //                                DateTime = Convert.ToDateTime(ja[i]["DateTime"]),
            //                                UpdateDatetime = DateTime.Now,
            //                                RefNo = strTobk2LineItemNo,
            //                                LineItemNo = intMaxLineItemNo,                                         
            //                                StatusCode = "USE",
            //                                UpdateBy = ja[0]["DriverCode"] == null ? "" : Modfunction.SQLSafe(ja[0]["DriverCode"].ToString()),                                          
            //                                Description = "PICKUP"
            //                            });



                               
                             
            //                    }
                            

            //                }
            //                Result = 1;
            //            }
            //        }
            //    }
            //}
            //catch { throw; }
            return Result;
        }
        public int UpdateAll_tjms2(Tobk request)
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
                                int ChargeBerthQty;
                                int ChargeLiftingQty;
                                if (ja[i]["TrxNo"] == null || ja[i]["TrxNo"].ToString() == "")
                                { continue; }
                                string strTrxNo = ja[i]["TrxNo"].ToString();
                                string strLineItemNo = ja[i]["LineItemNo"].ToString();
                                string SignedByName = ja[i]["SignedByName"].ToString();
                                string SignedByNric = ja[i]["SignedByNric"].ToString();
                                string SignedByDesignation = ja[i]["SignedByDesignation"].ToString();
                                string CompanyName = ja[i]["CompanyName"].ToString();
                                string strDateCompleted = ja[i]["DateCompleted"].ToString();
                                string OfficeInChargeName = ja[i]["OfficeInChargeName"].ToString();
                                if (ja[i]["ChargeBerthQty"].ToString() == "")
                                {
                                    ChargeBerthQty = 0;
                                }
                                else
                                {
                                     ChargeBerthQty = int.Parse(ja[i]["ChargeBerthQty"].ToString());
                                }

                                if (ja[i]["ChargeLiftingQty"].ToString() == "")
                                {
                                    ChargeLiftingQty = 0;
                                }
                                else
                                {
                                    ChargeLiftingQty = int.Parse(ja[i]["ChargeLiftingQty"].ToString());
                                }

                                string ChargeOther = ja[i]["ChargeOther"].ToString();
                                DateTime dt = DateTime.Now;
                                if (strDateCompleted != "" && strDateCompleted != null) {
                                    strDateCompleted = strDateCompleted +" "+ dt.GetDateTimeFormats('t')[0].ToString();
                                }
                                string str;
                                if (strLineItemNo != "0")
                                {

                                    str = " SignedByName = " + Modfunction.SQLSafeValue(SignedByName) + ",SignedByNric= " + Modfunction.SQLSafeValue(SignedByNric) + ",SignedByDesignation= " + Modfunction.SQLSafeValue(SignedByDesignation) + ",DateCompleted=" + Modfunction.SQLSafeValue(strDateCompleted) +  ",ChargeBerthQty="+ ChargeBerthQty + ",ChargeLiftingQty="+ ChargeLiftingQty + ",ChargeOther=" + Modfunction.SQLSafeValue(ChargeOther) + "";
                                    db.Update("tjms1",
                                           str,
                                           " TrxNo='" + strTrxNo + "' ");

                                    str =  "OfficeInChargeName= " + Modfunction.SQLSafeValue(OfficeInChargeName) + "";
                                    db.Update("tjms2",
                                           str,
                                           " TrxNo='" + strTrxNo + "' ");

                                    str = " CompanyName = " + Modfunction.SQLSafeValue(CompanyName) + "";
                                    db.Update("saco1",
                                           str
                                           );
                                }

                            }
                            Result = 1;
                        }
                    }          
                }
            }
            catch { throw; }
            return Result;
        }

    }
}
